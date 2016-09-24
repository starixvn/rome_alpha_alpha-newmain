namespace Alpha.Main.Site.Handlers
{
    using System;
    using Main.Models;
    using Main.Models.CustomerProfiles;
    using Main.Models.Enumerations;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.Password;
    using Requests.Password;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels;
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
        private const string HashKeySessionKey = "ph";

        private readonly PasswordService passwordService;
        private readonly IUserSession userSession;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly CustomerProfileService customerProfileService;

        public PasswordHandler(
            PasswordService passwordService, 
            IUserSession userSession,
            CustomerProfileService customerProfileService,
            SessionManager sessionManager,
            Encryptor encryptor)
        {
            this.passwordService = passwordService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.customerProfileService = customerProfileService;
        }

        public Result Handle(ResetSubAccountPasswordCommand message)
        {
            var result = this.passwordService.ResetSubAccountPassword(
                new ResetSubAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = message.HashKey,
                    Ip = message.Ip,
                    IsCasinoDisabled = message.IsCasinoDisabled,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId
                });

            return result;
        }

        public PasswordViewModel Handle(ChangePasswordQuery message)
        {
           
            return new PasswordViewModel
            {
                HashKey = GetHasKey()
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
            var result = this.passwordService.ResetMainAccountPassword(
                new ResetMainAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.ActorName,
                    CustId = message.CustId,
                    HashKey = message.HashKey,
                    Ip = message.Ip,
                    IsCasinoDisabled = message.IsCasinoDisabled,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId,
                    IsCasinoSync = message.IsCasinoSync,
                    IsResetMemberPassword = Enumeration.FromValue<UserLevel>((byte)message.CustRole).IsMemberLevel()
                });

            return result;
        }

        public Result Handle(ChangePasswordCommand message)
        {
            var result = new Result();
            if (message.IsSubAccount)
            {
                result = this.passwordService.ChangeSubAccountPassword(
                new ChangeSubAccountPasswordParam
                {
                    CustId = message.CustId,
                    HashKey = message.HashKey,
                    Ip = message.Ip,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    SubAccountId = message.SubAccountId,
                    CurrentPassword = message.CurrentPassword,
                    Nickname = message.NickName,
                    SubUsername = message.SubAccountUsername,
                    Username = message.Username,
                });
            }
            else
            {
                result = this.passwordService.ChangeMainAccountPassword(new ChangeMainAccountPasswordParam
                {
                    ActorId = message.ActorId,
                    ActorName = message.Username,
                    CurrentPassword = message.CurrentPassword,
                    CustId = message.CustId,
                    HashKey = message.HashKey,
                    Ip = message.Ip,
                    IsCasinoDisabled = message.IsCasinoDisabled,
                    IsCasinoSync = message.IsCasinoSync,
                    LanguageKey = message.LanguageKey,
                    NewPassword = message.NewPassword,
                    NickName = message.NickName
                });
            }

            if(result.IsSuccessful)
            {
                // Update is first login status
                this.userSession.IsLogged = true;

                // Update last change pwd date
                this.userSession.IsPasswordExpired = false;
            }

            return result;
        }

        public ForceChangePasswordViewModel Handle(ForceChangePasswordQuery message)
        {
            var profile = customerProfileService.GetProfilePicture(this.userSession.CustId);

            return new ForceChangePasswordViewModel
            {
                HashKey = GetHasKey(),
                IsDefaultAvatar = profile.IsDefaultAvatar,
                Username = this.userSession.UserName,
                AlternateUsername = this.BuildAlternateUsername(message),
                UsernameShower = this.BuildUsernameShower(message),
                EmbedImage = this.GenerateImage(profile)
            };
        }

        public PasswordViewModel Handle(ResetMainAccountPasswordQuery message)
        {
            var viewModel = Handle(new ChangePasswordQuery());

            viewModel.CustId = message.CustId;
            viewModel.CustRole = message.CustRole;

            return viewModel;
        }

        public PasswordViewModel Handle(ResetSubAccountPasswordQuery message)
        {
            var viewModel = Handle(new ChangePasswordQuery());

            viewModel.SubAccountId = message.SubAccountId;

            return viewModel;
        }

        private string GenerateImage(ProfilePicture profilePicture)
        {
            if (profilePicture.BinaryAvatar != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.BinaryAvatar)}";
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
                return "(" + message.SubUserName + ")";
            }
            else
            {
                return string.IsNullOrEmpty(message.NickName) ? string.Empty : "(" + message.NickName + ")";
            }
        }
    }
}