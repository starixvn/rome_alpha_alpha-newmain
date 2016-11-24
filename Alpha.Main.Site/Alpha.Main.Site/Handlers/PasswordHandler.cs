namespace Alpha.Main.Site.Handlers
{
    using System;
    using Alpha.SecurityCode.Services;
    using AppResources;
    using Main.Models;
    using Main.Models.CustomerProfiles;
    using Main.Models.Enumerations;
    using MediatR;
    using Params.Password;
    using Requests.Password;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels.Security;

    public class PasswordHandler :
         IRequestHandler<ChangePasswordCommand, Result>,
         IRequestHandler<ResetMainAccountPasswordCommand, Result>,
         IRequestHandler<ResetSubAccountPasswordCommand, Result>,
         IRequestHandler<ChangePasswordQuery, PasswordViewModel>,
         IRequestHandler<ForceChangePasswordQuery, ForceChangePasswordViewModel>,
         IRequestHandler<ResetMainAccountPasswordQuery, PasswordViewModel>,
         IRequestHandler<ResetSubAccountPasswordQuery, PasswordViewModel>
    {
        private const string HashKeySessionKey = "Alpha.Main.PasswordHandler.PasswordHash";

        private readonly PasswordService passwordService;
        private readonly IUserSession userSession;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly CustomerProfileService customerProfileService;
        private readonly SecurityCodeChecker securityCodeChecker;

        public PasswordHandler(
            PasswordService passwordService,
            IUserSession userSession,
            CustomerProfileService customerProfileService,
            SessionManager sessionManager,
            Encryptor encryptor,
            SecurityCodeChecker securityCodeChecker)
        {
            this.passwordService = passwordService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.customerProfileService = customerProfileService;
            this.securityCodeChecker = securityCodeChecker;
        }

        public Result Handle(ResetSubAccountPasswordCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!HasUpdatePermission(true, true))
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = this.passwordService.ResetSubAccountPassword(
                new ResetSubAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = this.sessionManager.Get<string>(HashKeySessionKey),
                    IP = message.IP,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId
                });

            return result;
        }

        public PasswordViewModel Handle(ChangePasswordQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return new PasswordViewModel
            {
                CustRoleId = message.CustRoleId,
                HashKey = GetHasKey(),
                EnableSecurityCode = securityCodeChecker.IsEnableSecurityCode()
            };
        }

        private string GetHasKey()
        {
            var hashKey = this.encryptor.RandomString(new Random().Next(4, 8));

            this.sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        public Result Handle(ResetMainAccountPasswordCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!HasUpdatePermission(true))
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = this.passwordService.ResetMainAccountPassword(
                new ResetMainAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = this.sessionManager.Get<string>(HashKeySessionKey),
                    IP = message.IP,
                    IsCasinoDisabled = message.IsCasinoDisabled,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId,
                    IsCasinoSync = message.IsCasinoSync,
                    IsResetMemberPassword = Enumeration.FromValue<UserLevel>((byte)message.CustRoleId).IsMemberLevel()
                });

            return result;
        }

        public Result Handle(ChangePasswordCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = message.IsSubAccount ? ChangeSubAccountPassword(message) : ChangeMainAccountPassword(message);

            if (result.IsSuccessful)
            {
                //// Update is first login status
                this.userSession.IsLogged = true;

                //// Update last change pwd date
                this.userSession.IsPasswordExpired = false;
            }

            return result;
        }

        private Result ChangeMainAccountPassword(ChangePasswordCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return this.passwordService.ChangeMainAccountPassword(new ChangeMainAccountPasswordParam
            {
                ActorId = message.ActorId,
                ActorName = message.Username,
                CurrentPassword = message.CurrentPassword,
                CustId = message.CustId,
                HashKey = this.sessionManager.Get<string>(HashKeySessionKey),
                IP = message.IP,
                IsCasinoDisabled = message.IsCasinoDisabled,
                IsCasinoSync = message.IsCasinoSync,
                LanguageKey = message.LanguageKey,
                NewPassword = message.NewPassword,
                NickName = message.NickName
            });
        }

        private Result ChangeSubAccountPassword(ChangePasswordCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return this.passwordService.ChangeSubAccountPassword(
            new ChangeSubAccountPasswordParam
            {
                CustId = message.CustId,
                HashKey = this.sessionManager.Get<string>(HashKeySessionKey),
                IP = message.IP,
                LanguageKey = message.LanguageKey,
                NewPassword = message.NewPassword,
                SubAccountId = message.SubAccountId,
                CurrentPassword = message.CurrentPassword,
                Nickname = message.NickName,
                SubUsername = message.SubAccountUsername,
                Username = message.Username,
            });
        }

        public ForceChangePasswordViewModel Handle(ForceChangePasswordQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var profile = customerProfileService.GetProfilePicture(this.userSession.CustId);

            return new ForceChangePasswordViewModel
            {
                HashKey = GetHasKey(),
                IsDefaultAvatar = profile.UseDefaultAvatar,
                Username = this.userSession.UserName,
                AlternateUsername = this.BuildAlternateUsername(message),
                UsernameShower = this.BuildUsernameShower(message),
                EmbedImage = this.GenerateImage(profile)
            };
        }

        public PasswordViewModel Handle(ResetMainAccountPasswordQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var viewModel = Handle(new ChangePasswordQuery());

            viewModel.CustId = message.CustId;
            viewModel.CustRoleId = message.CustRoleId;
            viewModel.IsCasinoSync = message.IsCasinoSync;
            viewModel.Title = BuildResetPasswordTitle(message.Username);

            return viewModel;
        }

        public PasswordViewModel Handle(ResetSubAccountPasswordQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var viewModel = Handle(new ChangePasswordQuery());

            viewModel.SubAccountId = message.SubAccountId;
            viewModel.Title = BuildResetPasswordTitle(message.Username);

            return viewModel;
        }

        private string BuildResetPasswordTitle(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Resources.lblresetpassword;
            }

            return $"{Resources.lblresetpassword} - {username}";
        }

        private string GenerateImage(ProfilePicture profilePicture)
        {
            if (profilePicture.ImageData != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.ImageData)}";
            }

            return string.Empty;
        }

        private string BuildUsernameShower(ForceChangePasswordQuery message)
        {
            if (message.IsSubAcc)
            {
                return message.SubUserName;
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? message.UserName : message.NickName;
            }
        }

        private string BuildAlternateUsername(ForceChangePasswordQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {message.SubUserName})";
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? string.Empty : $"({message.NickName})";
            }
        }

        private bool HasUpdatePermission(bool isResetPassword = false, bool isResetSubAccount = false)
        {
            if (userSession.IsInternal && !isResetSubAccount)
            {
                return false;
            }

            if (userSession.IsLimitedHostPermisionOnSubAccountFunction)
            {
                return false;
            }

            if (isResetPassword
                && (userSession.IsSubAcc && !userSession.SubPageAccess.UpdateAccount))
            {
                return false;
            }

            return true;
        }
    }
}