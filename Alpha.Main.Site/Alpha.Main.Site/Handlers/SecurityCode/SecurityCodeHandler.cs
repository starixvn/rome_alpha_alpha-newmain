namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using AppResources;
    using Configurations;
    using Encryption;
    using Fanex.Security;
    using Main.Helpers;
    using Main.Models.CustomerProfiles;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Services;
    using Sessions;
    using Sunplus.Agent.Utilities;
    using ViewModels.Security;
    public class SecurityCodeHandler : SecurityCodeBaseHandler,
         IRequestHandler<LoginSecurityCodeQuery, LoginSecurityCodeViewModel>,
         IRequestHandler<BaseSecurityCodeQuery, BaseSecurityCodeViewModel>,
         IRequestHandler<ChangeSecurityCodeFirstLoginCommand, Result>,
         IRequestHandler<ChangeSecurityCodeByForcingCommand, Result>,
         IRequestHandler<ChangeUserOwnSecurityCodeCommand, Result>,
         IRequestHandler<ResetSecurityCodeCommand, Result>,
         IRequestHandler<VerifySecurityCodeCommand, Result>
    {
        private readonly CustomerProfileService customerProfileService;

        private string PH
        {
            get
            {
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["ph"] as string))
                {
                    System.Web.HttpContext.Current.Session["ph"] = Encryption.RandomString(new Random().Next(4, 8));
                }

                return System.Web.HttpContext.Current.Session["ph"] + string.Empty;
            }
        }

        private string GenerateSecurityToken(int size = 6)
        {
            return Cryptography.RandomString(size);
        }

        private string GetRC4Decryption(string encryption)
        {
            if (string.IsNullOrEmpty(encryption))
            {
                return string.Empty;
            }

            return Alpha.Main.Encryption.Encryptor.RC4(this.PH, Alpha.Main.Encryption.Encryptor.HexDecode(encryption));
        }

        public SecurityCodeHandler(
           SecurityCodeService securityCodeService,
           CustomerProfileService customerProfileService,
           ISecurityCodeSession securityCodeSession = null) : base(securityCodeService, securityCodeSession)
        {
            this.customerProfileService = customerProfileService;
        }

        public BaseSecurityCodeViewModel Handle(BaseSecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var profile = customerProfileService.GetProfilePicture(base.securityCodeSession.CustId);

            string securityCodeToken = this.GenerateSecurityToken();
            base.securityCodeSession.SecurityCodeToken = securityCodeToken;

            var SecuritySalt = base.securityCodeSession.SubAccId > 0
                ? base.MD5Salt(base.securityCodeSession.SubAccId)
                : base.MD5Salt(base.securityCodeSession.CustId);

            return new BaseSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = SecuritySalt,
                PH = this.PH,
                RootUrl = AppPathHelper.GetAppPath(AppSettings.SiteMainAppPath, "~")
            };
        }


        public LoginSecurityCodeViewModel Handle(LoginSecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var profile = customerProfileService.GetProfilePicture(base.securityCodeSession.CustId);

            string securityCodeToken = this.GenerateSecurityToken();
            base.securityCodeSession.SecurityCodeToken = securityCodeToken;

            var SecuritySalt = base.securityCodeSession.SubAccId > 0
                ? base.MD5Salt(base.securityCodeSession.SubAccId)
                : base.MD5Salt(base.securityCodeSession.CustId);

            return new LoginSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = SecuritySalt,
                PH = this.PH,
                IsDefaultAvatar = profile.IsDefaultAvatar,
                Username = message.UserName,
                AlternateUsername = this.BuildAlternateUsername(message),
                UsernameShower = this.BuildUsernameShower(message),
                EmbedImage = this.GenerateImage(profile),
                RootUrl = AppPathHelper.GetAppPath(AppSettings.SiteMainAppPath, "~")
            };
        }

        public Result Handle(ChangeSecurityCodeFirstLoginCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (base.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (errorMessage.Length != 0)
            {
                return new Result { ErrorCode = -1, ErrorMessage = errorMessage };
            }

            return base.ChangeSecurityCodeFirstLogin(message);

        }

        public Result Handle(ChangeSecurityCodeByForcingCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (base.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.OldSecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (errorMessage.Length != 0)
            {
                return new Result { ErrorCode = -1, ErrorMessage = errorMessage };
            }

            return base.ChangeSecurityCodeByForcing(message);
        }

        public Result Handle(ChangeUserOwnSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (base.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.OldSecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (errorMessage.Length != 0)
            {
                return new Result { ErrorCode = -1, ErrorMessage = errorMessage };
            }

            return base.ChangeUserOwnSecurityCode(message);

        }

        public Result Handle(ResetSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            bool isResetSubAccount = message.SubAccId.HasValue && message.SubAccId.Value > 0;

            if (base.securityCodeSession.IsInternal && !isResetSubAccount)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (errorMessage.Length != 0)
            {
                return new Result { ErrorCode = -1, ErrorMessage = errorMessage };
            }

            Result result = new Result();
            if (message.CustId.HasValue && message.CustId.Value > 0)
            {
                result = base.ResetDownlineSecurityCode(message);
            }
            else if (isResetSubAccount)
            {
                result = base.ResetSubAccountSecurityCode(message);
            }

            return result;
        }

        public Result Handle(VerifySecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (base.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.informSecCode };
            }

            return base.VerifySecurityCode(message);
        }

        private string GenerateImage(ProfilePicture profilePicture)
        {
            if (profilePicture.BinaryAvatar != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.BinaryAvatar)}";
            }

            return string.Empty;
        }

        private string BuildUsernameShower(LoginSecurityCodeQuery message)
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

        private string BuildAlternateUsername(LoginSecurityCodeQuery message)
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