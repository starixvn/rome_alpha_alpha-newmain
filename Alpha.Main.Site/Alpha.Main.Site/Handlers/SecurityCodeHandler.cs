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
         IRequestHandler<ChangeSecurityCodeByItselfCommand, Result>,
         IRequestHandler<ForceChangeSecurityCodeWhenLoginCommand, Result>,
         IRequestHandler<ForceChangeSecurityCodeTheFirstLoginCommand, Result>,
         IRequestHandler<ResetSecurityCodeCommand, Result>,
         IRequestHandler<VerifySecurityCodeCommand, Result>
    {
        public SecurityCodeHandler(
           SecurityCodeService securityCodeService,
           ISecurityCodeSession securityCodeSession = null) : base(securityCodeService, securityCodeSession)
        {
        }
             private void ResetToken()
        {
            this.securityCodeSession.SecurityCodeToken = string.Empty;
            this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
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

        public Result Handle(ForceChangeSecurityCodeTheFirstLoginCommand message)
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

            return base.ChangeSecurityCodeFirstLogin(message);

        }

        public Result Handle(ForceChangeSecurityCodeWhenLoginCommand message)
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

            return base.ForceChangeSecurityCodeWhenLogin(message);
        }

        public Result Handle(ChangeSecurityCodeByItselfCommand message)
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

            return base.ChangeSecurityCodeByItself(message);

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

            Result result = new Result();
            if (message.CustId.HasValue && message.CustId.Value > 0)
            {
                result =  base.ResetDownlineSecurityCode(message);
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

            Result result = base.VerifySecurityCodeForImportantAction(message);
            if (result.ErrorCode == 0)
            {
                this.ResetToken();
            }

            return result;
        }
    }
}