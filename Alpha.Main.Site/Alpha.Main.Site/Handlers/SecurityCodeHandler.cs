namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using AppResources;
    using Encryption;
    using Fanex.Security;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sessions;
    using System;
    using ViewModels.Security;

    public class SecurityCodeHandler :
         IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>,
         IRequestHandler<UpdateChangeSecurityCodeCommand, Result>,
         IRequestHandler<UpdateForceChangeSecurityCodeByUpline, Result>,
         IRequestHandler<UpdateForceChangeSecurityCodeTheFirstLogin, Result>,
         IRequestHandler<ResetSecurityCodeCommand, Result>,
         IRequestHandler<VerifySecurityCodeCommand, Result>
    {
        private ISecurityCodeSession securityCodeSession;
        private SecurityCodeService securityCodeService;

        private string PH
        {
            get
            {
                return System.Web.HttpContext.Current.Session["ph"] + string.Empty;
            }
        }

        private string ClientIP
        {
            get
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                    ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string[] ips = string.IsNullOrEmpty(ip) ? new string[] { string.Empty } : ip.Split(',');

                return ips[0];
            }
        }

        public SecurityCodeHandler(
           SecurityCodeService securityCodeService,
           ISecurityCodeSession securityCodeSession = null)
        {
            this.securityCodeService = securityCodeService;
            this.securityCodeSession = securityCodeSession;
        }

        public SecurityCodeViewModel Handle(SecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            string securityCodeToken = this.GenerateSecurityToken();
            this.securityCodeSession.SecurityCodeToken = securityCodeToken;

            var SecuritySalt = this.securityCodeSession.SubAccId > 0
                ? this.securityCodeService.MD5Salt(this.securityCodeSession.SubAccId)
                : this.securityCodeService.MD5Salt(this.securityCodeSession.CustId);

            return new SecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = SecuritySalt
            };
        }

        public Result Handle(UpdateChangeSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = GetRC4Decryption(message.ConfirmSecurityCode);

            //// validate request
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.OldSecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            return this.securityCodeService.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = this.securityCodeSession.CustId,
                    UserId = this.securityCodeSession.UserId,
                    OldSecurityCode = message.OldSecurityCode,
                    SecurityCode = message.SecurityCode,
                    SecurityCodeToken = message.SecurityCodeToken,
                    Language = this.securityCodeSession.Language,
                    IP = this.ClientIP,
                    SubAccountId = this.securityCodeSession.SubAccId
                });
        }

        public Result Handle(UpdateForceChangeSecurityCodeTheFirstLogin message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = GetRC4Decryption(message.ConfirmSecurityCode);

            //// validate request
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            return this.securityCodeService.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = this.securityCodeSession.CustId,
                    UserId = this.securityCodeSession.UserId,
                    SecurityCode = message.SecurityCode,
                    IP = this.ClientIP,
                    Language = this.securityCodeSession.Language,
                    SubAccountId = this.securityCodeSession.SubAccId
                });
        }

        public Result Handle(UpdateForceChangeSecurityCodeByUpline message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = GetRC4Decryption(message.ConfirmSecurityCode);

            //// validate request
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.OldSecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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

            Result result = this.securityCodeService.CompareSecurityCodeBetweenWebAndDB(
                new CheckSecurityCodeParams
                {
                    CustId = this.securityCodeSession.CustId,
                    SecurityCode = message.OldSecurityCode,
                    SecurityCodeToken = message.SecurityCodeToken,
                    Language = this.securityCodeSession.Language,
                    IP = this.ClientIP,
                    SubAccountId = this.securityCodeSession.SubAccId
                });

            if (result.ErrorCode != 0)
            {
                return result;
            }

            return this.securityCodeService.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = this.securityCodeSession.CustId,
                    UserId = this.securityCodeSession.UserId,
                    OldSecurityCode = message.OldSecurityCode,
                    SecurityCode = message.SecurityCode,
                    SecurityCodeToken = message.SecurityCodeToken,
                    IP = this.ClientIP,
                    Language = this.securityCodeSession.Language,
                    SubAccountId = this.securityCodeSession.SubAccId
                });
        }

        public Result Handle(ResetSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            bool isResetSubAccount = message.SubAccountId > 0;
            if (this.securityCodeSession.IsInternal && !isResetSubAccount)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            message.SecurityCode = GetRC4Decryption(message.SecurityCode);
            message.ConfirmSecurityCode = GetRC4Decryption(message.ConfirmSecurityCode);

            //// validate request
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                errorMessage = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!this.securityCodeService.ValidateSecurityCode(message.SecurityCode))
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
            return this.securityCodeService.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = this.securityCodeSession.CustId,
                    UserId = this.securityCodeSession.UserId,
                    SecurityCode = message.SecurityCode,
                    IP = this.ClientIP,
                    Language = this.securityCodeSession.Language,
                    SubAccountId = this.securityCodeSession.SubAccId
                });
        }

        public Result Handle(VerifySecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (this.securityCodeSession.IsInternal)
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized };
            }

            if (string.IsNullOrEmpty(message.SecurityCode))
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.informSecCode };
            }

            Result result = this.securityCodeService.CompareSecurityCodeBetweenWebAndDB(
               new CheckSecurityCodeParams
               {
                   CustId = this.securityCodeSession.CustId,
                   SecurityCode = message.SecurityCode,
                   SecurityCodeToken = message.SecurityCodeToken,
                   Language = this.securityCodeSession.Language,
                   IP = this.ClientIP,
                   SubAccountId = this.securityCodeSession.SubAccId
               });

            if (result.ErrorCode == 0)
            {
                this.securityCodeSession.SecurityCodeToken = string.Empty;
                this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return result;
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

            return Encryptor.RC4(this.PH, Encryptor.HexDecode(encryption));
        }
    }
}