namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Params;
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.SecurityCode.Services;
    using AppResources;
    using Main.Models;
    using Main.Models.Enumerations;
    using MediatR;
    using Params.Password;
    using Requests.Password;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using System;
    using ViewModels.Security;
    using Web.Helpers;

    public class PasswordHandler :
         IRequestHandler<ChangePasswordCommand, Result>,
         IRequestHandler<ResetMainAccountPasswordCommand, Result>,
         IRequestHandler<ResetSubAccountPasswordCommand, Result>,
         IRequestHandler<ChangePasswordQuery, PasswordViewModel>,
         IRequestHandler<ForceChangePasswordQuery, ForceChangePasswordViewModel>,
         IRequestHandler<ResetMainAccountPasswordQuery, PasswordViewModel>,
         IRequestHandler<ResetSubAccountPasswordQuery, PasswordViewModel>,
         IRequestHandler<SkipForceChangePasswordCommand, Result>
    {
        private const string HashKeySessionKey = "Alpha.Main.PasswordHandler.PasswordHash";

        private readonly PasswordService passwordService;
        private readonly IUserSession userSession;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly ProfilePictureServices customerProfileService;
        private readonly SecurityCodeChecker securityCodeChecker;
        private readonly IUser user;
        private readonly Func<int> getInsertProfilePictureSiteMode;

        public PasswordHandler(
            PasswordService passwordService,
            IUserSession userSession,
            ProfilePictureServices customerProfileService,
            SessionManager sessionManager,
            Encryptor encryptor,
            SecurityCodeChecker securityCodeChecker,
            IUser user)
           : this(passwordService, userSession, customerProfileService, sessionManager, encryptor, securityCodeChecker, user, null)
        {
        }

        internal PasswordHandler(
            PasswordService passwordService,
            IUserSession userSession,
            ProfilePictureServices customerProfileService,
            SessionManager sessionManager,
            Encryptor encryptor,
            SecurityCodeChecker securityCodeChecker,
            IUser user,
            Func<int> getInsertProfilePictureSiteMode)
        {
            this.passwordService = passwordService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.customerProfileService = customerProfileService;
            this.securityCodeChecker = securityCodeChecker;
            this.user = user;
            this.getInsertProfilePictureSiteMode = getInsertProfilePictureSiteMode ??
             SimpleServiceLocator.GetService<Func<int>>();
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

            return this.passwordService.ResetSubAccountPassword(
                new ResetSubAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = this.sessionManager.Get<string>(HashKeySessionKey),
                    IP = message.IP,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId,
                    CustLevel = Enumeration.FromValue<UserLevel>(message.CustRoleId),
                    ServerId = message.ServerId
                });
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
            var hashKey = this.encryptor.RandomString(4);

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

            var custLevel = Enumeration.FromValue<UserLevel>(message.CustRoleId);

            return this.passwordService.ResetMainAccountPassword(
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
                    IsResetMemberPassword = custLevel.IsMemberLevel(),
                    CustLevel = custLevel,
                    ServerId = message.ServerId
                });
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

        public Result Handle(SkipForceChangePasswordCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!message.IsSkipForceChangePassword && 
                passwordService.SkipForceChangePassword(new SkipForceChangePasswordParam
                {
                    CustId = message.CustId,
                    SubAccountId = message.SubAccId,
                    LanguageKey = message.Language,
                    IP = message.IP
                }).IsNotOk)
            {
                return Result.CreateErrorResult("Can not skip force change Password");
            }

            //// Update is first login status
            this.userSession.IsLogged = true;

            //// Update last change pwd date
            this.userSession.IsPasswordExpired = false;

            return new Result { Code = 0 };
        }

        private Result ChangeMainAccountPassword(ChangePasswordCommand message)
        {
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

            var profile = customerProfileService.GetActorProfilePicture(this.userSession.CustId);

            if (profile == null || string.IsNullOrEmpty(profile.ImageBase64String))
            {
                var result = customerProfileService.UpdateActorProfilePicture(
                    new UpdateActorProfilePictureParam(
                        this.userSession.CustId,
                        getInsertProfilePictureSiteMode(),
                        this.userSession.UserLoginIp));

                profile = result.IsOk ? customerProfileService.GetActorProfilePicture(this.userSession.CustId) : new ActorProfilePictureModel();
            }

            return new ForceChangePasswordViewModel
            {
                HashKey = GetHasKey(),
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                Username = this.userSession.UserName,
                AlternateUsername = this.BuildAlternateUsername(message),
                UsernameShower = this.BuildUsernameShower(message),
                EmbedImage = profile.ImageBase64String,
                ProfileImageId = profile.ImageId,
                IsSkipForceChangePassword = message.IsSkipForceChangePassword
            };
        }

        public PasswordViewModel Handle(ResetMainAccountPasswordQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var viewModel = Handle(new ChangePasswordQuery(userSession));

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

            var viewModel = Handle(new ChangePasswordQuery(userSession));

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

            if (isResetSubAccount && !user.IsAdminCanUpdateSubAccount)
            {
                return false;
            }

            return true;
        }
    }
}