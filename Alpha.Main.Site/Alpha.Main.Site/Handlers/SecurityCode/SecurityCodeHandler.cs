namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using AppResources;
    using Fanex.Security;
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
        private const string HashKeySessionKey = "ph";
        private readonly CustomerProfileService customerProfileService;
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

            var profile = customerProfileService.GetProfilePicture(this.securityCodeSession.CustId);

            string securityCodeToken = this.GenerateSecurityToken();
            this.securityCodeSession.SecurityCodeToken = securityCodeToken;

            var securitySalt = this.securityCodeSession.SubAccId > 0
                ? this.MD5Salt(this.securityCodeSession.SubAccId)
                : this.MD5Salt(this.securityCodeSession.CustId);

            var model = new BaseSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = securitySalt,
                HashKey = this.HashKey,
                CustRoleId = message.RoleId
            };

            return model;
        }

        public LoginSecurityCodeViewModel Handle(LoginSecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var profile = customerProfileService.GetProfilePicture(this.securityCodeSession.CustId);

            string securityCodeToken = this.GenerateSecurityToken();
            this.securityCodeSession.SecurityCodeToken = securityCodeToken;

            var securitySalt = this.securityCodeSession.SubAccId > 0
                ? this.MD5Salt(this.securityCodeSession.SubAccId)
                : this.MD5Salt(this.securityCodeSession.CustId);

            return new LoginSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = securitySalt,
                HashKey = this.HashKey,
                CustRoleId = message.RoleId,
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

            if (this.securityCodeSession.IsInternal)
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
            else if (!this.ValidateSecurityCode(message.SecurityCode))
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

            return this.ChangeSecurityCodeFirstLogin(message);
        }

        public Result Handle(ChangeSecurityCodeByForcingCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
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
            else if (!this.ValidateSecurityCode(message.SecurityCode))
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

            return this.ChangeSecurityCodeByForcing(message);
        }

        public Result Handle(ChangeUserOwnSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
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
            else if (!this.ValidateSecurityCode(message.SecurityCode))
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

            return this.ChangeUserOwnSecurityCode(message);
        }

        public Result Handle(ResetSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            bool isResetSubAccount = message.SubAccId.HasValue && message.SubAccId.Value > 0;

            if (this.securityCodeSession.IsInternal && !isResetSubAccount)
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
            else if (!this.ValidateSecurityCode(message.SecurityCode))
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
                result = this.ResetDownlineSecurityCode(message);
            }
            else if (isResetSubAccount)
            {
                result = this.ResetSubAccountSecurityCode(message);
            }

            return result;
        }

        public Result Handle(VerifySecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                return new Result { ErrorCode = -1, ErrorMessage = Security.informSecCode };
            }

            return this.VerifySecurityCode(message);
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