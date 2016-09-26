namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using AppResources;
    using Configurations;
    using Fanex.Security;
    using Main.Helpers;
    using Main.Models.CustomerProfiles;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Services;
    using Sessions;
    using ViewModels.Security;
    using Sunplus.Agent.Utilities;

    public class SecurityCodeHandler : SecurityCodeBaseHandler,
         IRequestHandler<LoginSecurityCodeQuery, LoginSecurityCodeViewModel>,
         IRequestHandler<BaseSecurityCodeQuery, BaseSecurityCodeViewModel>,
         IRequestHandler<ChangeSecurityCodeFirstLoginCommand, Result>,
         IRequestHandler<ChangeSecurityCodeByForcingCommand, Result>,
         IRequestHandler<ChangeUserOwnSecurityCodeCommand, Result>,
         IRequestHandler<ResetSecurityCodeCommand, Result>,
         IRequestHandler<VerifySecurityCodeCommand, Result>
    {
        private const string HashKeySessionKey = "ph";
        private readonly CustomerProfileService customerProfileService;
        private readonly CachingManager cacheManager;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;

        public SecurityCodeHandler(
          SecurityCodeService securityCodeService,
          CustomerProfileService customerProfileService,
          SessionManager sessionManager,
          Encryptor encryptor,
          ISecurityCodeSession securityCodeSession = null) : base(securityCodeService, securityCodeSession)
        {
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.customerProfileService = customerProfileService;
        }

        private string HashKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.sessionManager.Get<string>(HashKeySessionKey)))
                {
                    var hashKey = this.encryptor.RandomString(new Random().Next(4, 8));

                    this.sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return this.sessionManager.Get<string>(HashKeySessionKey) + string.Empty;
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

            return this.encryptor.RC4(this.HashKey, this.encryptor.HexDecode(encryption));
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
                HashKey = this.HashKey
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
                HashKey = this.HashKey,
                IsDefaultAvatar = profile.IsDefaultAvatar,
                Username = message.UserName,
                AlternateUsername = this.BuildAlternateUsername(message),
                UsernameShower = this.BuildUsernameShower(message),
                EmbedImage = this.GenerateImage(profile)
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
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = Security.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = Security.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = Security.alertErrorSecCodeConfirm;
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
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.OldSecurityCode))
            {
                errorMessage = Security.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = Security.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = Security.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = Security.alertErrorSecCodeConfirm;
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
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.OldSecurityCode))
            {
                errorMessage = Security.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = Security.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = Security.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = Security.alertErrorSecCodeConfirm;
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
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            message.SecurityCode = this.GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = this.GetRC4Decryption(message.ConfirmSecurityCode);

            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = Security.alertSecCodeEmpty;
            }
            else if (!base.ValidateSecurityCode(message.SecurityCode))
            {
                errorMessage = Security.lblSecCodeWrongRule;
            }
            else if (message.SecurityCode != message.ConfirmSecurityCode)
            {
                errorMessage = Security.alertErrorSecCodeConfirm;
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
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                return new Result { ErrorCode = -1, ErrorMessage = Security.informSecCode };
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