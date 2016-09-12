namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using AppResources;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sessions;
    using System;
    using ViewModels.Security;

    public class SecurityCodeHandler : SecurityCodeBaseHandler,
         IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>,
         IRequestHandler<UpdateChangeSecurityCodeCommand, Result>,
         IRequestHandler<UpdateForceChangeSecurityCodeByUpline, Result>,
         IRequestHandler<UpdateForceChangeSecurityCodeTheFirstLogin, Result>,
         IRequestHandler<ResetSecurityCodeCommand, Result>,
         IRequestHandler<VerifySecurityCodeCommand, Result>
    {
        
        public SecurityCodeHandler(
           SecurityCodeService securityCodeService,
           ISecurityCodeSession securityCodeSession = null) : base(securityCodeService, securityCodeSession)
        {
        }

        public SecurityCodeViewModel Handle(SecurityCodeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            string securityCodeToken = this.GenerateSecurityToken();
            base.securityCodeSession.SecurityCodeToken = securityCodeToken;

            var SecuritySalt = base.securityCodeSession.SubAccId > 0
                ? base.MD5Salt(base.securityCodeSession.SubAccId)
                : base.MD5Salt(base.securityCodeSession.CustId);

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

            if (base.securityCodeSession.IsInternal)
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

            Result result = base.CompareSecurityCodeBetweenWebAndDB(
              new CheckSecurityCodeParams
              {
                  CustId = base.securityCodeSession.CustId,
                  SecurityCode = message.OldSecurityCode,
                  SecurityCodeToken = message.SecurityCodeToken,
                  Language = base.securityCodeSession.Language,
                  IP = base.ClientIP,
                  SubAccountId = base.securityCodeSession.SubAccId
              });

            if (result.ErrorCode != 0)
            {
                return result;
            }

            return base.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = base.securityCodeSession.CustId,
                    UserId = base.securityCodeSession.UserId,
                    OldSecurityCode = message.OldSecurityCode,
                    SecurityCode = message.SecurityCode,
                    SecurityCodeToken = message.SecurityCodeToken,
                    Language = base.securityCodeSession.Language,
                    IP = base.ClientIP,
                    SubAccountId = base.securityCodeSession.SubAccId
                });
        }

        public Result Handle(UpdateForceChangeSecurityCodeTheFirstLogin message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (base.securityCodeSession.IsInternal)
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

            return base.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = base.securityCodeSession.CustId,
                    UserId = base.securityCodeSession.UserId,
                    SecurityCode = message.SecurityCode,
                    IP = base.ClientIP,
                    Language = base.securityCodeSession.Language,
                    SubAccountId = base.securityCodeSession.SubAccId
                });
        }

        public Result Handle(UpdateForceChangeSecurityCodeByUpline message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            if (base.securityCodeSession.IsInternal)
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

            Result result = base.CompareSecurityCodeBetweenWebAndDB(
                new CheckSecurityCodeParams
                {
                    CustId = base.securityCodeSession.CustId,
                    SecurityCode = message.OldSecurityCode,
                    SecurityCodeToken = message.SecurityCodeToken,
                    Language = base.securityCodeSession.Language,
                    IP = base.ClientIP,
                    SubAccountId = base.securityCodeSession.SubAccId
                });

            if (result.ErrorCode != 0)
            {
                return result;
            }

            return base.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = base.securityCodeSession.CustId,
                    UserId = base.securityCodeSession.UserId,
                    OldSecurityCode = message.OldSecurityCode,
                    SecurityCode = message.SecurityCode,
                    SecurityCodeToken = message.SecurityCodeToken,
                    IP = base.ClientIP,
                    Language = base.securityCodeSession.Language,
                    SubAccountId = base.securityCodeSession.SubAccId
                });
        }

        public Result Handle(ResetSecurityCodeCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            bool isResetSubAccount = message.SubAccountId > 0;
            if (base.securityCodeSession.IsInternal && !isResetSubAccount)
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

            return base.ChangeSecurityCode(
                new SecurityCodeParams
                {
                    CustId = base.securityCodeSession.CustId,
                    UserId = base.securityCodeSession.UserId,
                    SecurityCode = message.SecurityCode,
                    IP = base.ClientIP,
                    Language = base.securityCodeSession.Language,
                    SubAccountId = base.securityCodeSession.SubAccId
                });
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

            Result result = base.CompareSecurityCodeBetweenWebAndDB(
              new CheckSecurityCodeParams
              {
                  CustId = base.securityCodeSession.CustId,
                  SecurityCode = message.SecurityCode,
                  SecurityCodeToken = message.SecurityCodeToken,
                  Language = base.securityCodeSession.Language,
                  IP = base.ClientIP,
                  SubAccountId = base.securityCodeSession.SubAccId
              });

            if (result.ErrorCode != 0)
            {
                return result;
            }


            result = base.CompareSecurityCodeBetweenWebAndDB(
              new CheckSecurityCodeParams
              {
                  CustId = base.securityCodeSession.CustId,
                  SecurityCode = message.SecurityCode,
                  SecurityCodeToken = message.SecurityCodeToken,
                  Language = base.securityCodeSession.Language,
                  IP = base.ClientIP,
                  SubAccountId = base.securityCodeSession.SubAccId
              });

            if (result.ErrorCode == 0)
            {
                base.securityCodeSession.SecurityCodeToken = string.Empty;
                base.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return result;
        }
    }
}