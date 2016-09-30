namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using AppResources;
    using Fanex.Security;
    using Main.Models.CustomerProfiles;
    using Main.Models.Enumerations;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sessions;
    using Sunplus.Agent.Utilities;
    using ViewModels.Security;

    public class SecurityCodeHandler : IRequestHandler<LoginSecurityCodeQuery, LoginSecurityCodeViewModel>,
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
        private readonly SecurityCodeService securityCodeService;
        protected readonly ISecurityCodeSession SecurityCodeSession;

        public SecurityCodeHandler(
          SecurityCodeService securityCodeService,
          CustomerProfileService customerProfileService,
          SessionManager sessionManager,
          Encryptor encryptor,
          ISecurityCodeSession securityCodeSession = null)
        {
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.customerProfileService = customerProfileService;
            this.securityCodeService = securityCodeService;
            this.SecurityCodeSession = securityCodeSession;
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

            var profile = customerProfileService.GetProfilePicture(this.SecurityCodeSession.CustId);

            string securityCodeToken = this.GenerateSecurityToken();
            this.SecurityCodeSession.SecurityCodeToken = securityCodeToken;

            var securitySalt = this.SecurityCodeSession.SubAccId > 0
                ? this.securityCodeService.MD5Salt(this.SecurityCodeSession.SubAccId)
                : this.securityCodeService.MD5Salt(this.SecurityCodeSession.CustId);

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

            var profile = customerProfileService.GetProfilePicture(this.SecurityCodeSession.CustId);

            string securityCodeToken = this.GenerateSecurityToken();
            this.SecurityCodeSession.SecurityCodeToken = securityCodeToken;

            var securitySalt = this.SecurityCodeSession.SubAccId > 0
                ? this.securityCodeService.MD5Salt(this.SecurityCodeSession.SubAccId)
                : this.securityCodeService.MD5Salt(this.SecurityCodeSession.CustId);

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

            if (this.SecurityCodeSession.IsInternal)
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
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            Result result = this.UpdateSecurityCode(new SecurityCodeParams
            {
                CustId = message.CustId,
                SubAccId = message.SubAccId,
                UserId = message.UserId,
                SecurityCode = message.SecurityCode,
                Language = message.Language
            });

            if (result.ErrorCode.Equals(0))
            {
                this.SecurityCodeSession.SecurityCode = message.SecurityCode;
                this.ResetToken();
            }

            return result;
        }

        public Result Handle(ChangeSecurityCodeByForcingCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.SecurityCodeSession.IsInternal)
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
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            Result result = this.securityCodeService.CompareHashSecurityCodeBetweenWebAndDB(
               this.SecurityCodeSession.CustId,
               this.SecurityCodeSession.SubAccId,
               message.IsSubAcc,
               this.SecurityCodeSession.SecurityCodeToken,
               this.SecurityCodeSession.Language,
               message.OldSecurityCode);

            if (result.ErrorCode.Equals(0))
            {
                result = this.UpdateSecurityCode(new SecurityCodeParams
                {
                    CustId = message.CustId,
                    SubAccId = message.SubAccId,
                    UserId = message.UserId,
                    SecurityCode = message.SecurityCode,
                    Language = message.Language
                });
            }

            if (result.ErrorCode.Equals(0))
            {
                this.SecurityCodeSession.SecurityCode = message.SecurityCode;
                this.ResetToken();
            }

            return result;
        }

        public Result Handle(ChangeUserOwnSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.SecurityCodeSession.IsInternal)
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
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            Result result = this.securityCodeService.CompareHashSecurityCodeBetweenWebAndDB(
               this.SecurityCodeSession.CustId,
               this.SecurityCodeSession.SubAccId,
               message.IsSubAcc,
               this.SecurityCodeSession.SecurityCodeToken,
               this.SecurityCodeSession.Language,
               message.OldSecurityCode);

            if (result.ErrorCode.Equals(0))
            {
                result = this.UpdateSecurityCode(new SecurityCodeParams
                {
                    CustId = message.CustId,
                    SubAccId = message.SubAccId,
                    UserId = message.UserId,
                    SecurityCode = message.SecurityCode,
                    Language = message.Language
                });
            }

            if (result.ErrorCode.Equals(0))
            {
                this.ResetToken();
            }

            return result;
        }

        public Result Handle(ResetSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            bool isResetSubAccount = message.SubAccId.HasValue && message.SubAccId.Value > 0;

            if (this.SecurityCodeSession.IsInternal && !isResetSubAccount)
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
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            if (this.SecurityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = Security.NotAuthorized };
            }

            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                return new Result { ErrorCode = -1, ErrorMessage = Security.informSecCode };
            }

            Result result = this.securityCodeService.CompareHashSecurityCodeBetweenWebAndDB(
                message.CustId,
                message.SubAccId,
                message.IsSubAcc,
                this.SecurityCodeSession.SecurityCodeToken,
                this.SecurityCodeSession.Language,
                message.SecurityCode);

            if (result.ErrorCode == 0)
            {
                this.ResetToken();
            }

            return result;
        }

        private void ResetToken()
        {
            this.SecurityCodeSession.SecurityCodeToken = string.Empty;
            this.SecurityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SECURITYCODE_SUCCESS;
            this.SecurityCodeSession.IsForcedSecurityCode = false;
            Alpha.SecurityCode.Web.SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        private Result UpdateSecurityCode(SecurityCodeParams securityCodeParams)
        {
            Result result = securityCodeParams.SubAccId.Equals(0) ?
                this.securityCodeService.UpdateMainAccountHashSecurityCode(securityCodeParams) : this.securityCodeService.UpdateSubAccountHashSecurityCode(securityCodeParams);

            if (result != null && result.ErrorCode == 0)
            {
                result.ErrorMessage = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private Result ResetDownlineSecurityCode(ResetSecurityCodeCommand securityCodeParams)
        {
            var isResetMember = this.SecurityCodeSession.UserLevel.Equals(UserLevel.Agent);

            Result result = this.securityCodeService.UpdateMainAccountHashSecurityCode(new SecurityCodeParams
            {
                CustId = securityCodeParams.CustId.Value,
                UserId = this.SecurityCodeSession.UserId,
                SubAccId = this.SecurityCodeSession.SubAccId,
                IsResetMember = isResetMember,
                SecurityCode = securityCodeParams.SecurityCode,
                Language = securityCodeParams.Language,
            });

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = Security.alertSecCodeChangedOK;
            }

            return result;
        }

        private Result ResetSubAccountSecurityCode(ResetSecurityCodeCommand securityCodeParams)
        {
            Result result = this.securityCodeService.ResetSubAccountHashSecurityCode(new SecurityCodeParams
            {
                CustId = this.SecurityCodeSession.CustId,
                UserId = this.SecurityCodeSession.UserId,
                SubAccId = securityCodeParams.SubAccId.Value,
                HashSecurityCode = securityCodeParams.SecurityCode,
                Language = securityCodeParams.Language
            });

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = Security.alertSecCodeChangedOK;
            }

            return result;
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