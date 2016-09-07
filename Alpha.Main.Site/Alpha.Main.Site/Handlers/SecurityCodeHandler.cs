namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using Alpha.SecurityCode.Sessions;
    using Encryption;
    using Fanex.Security;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Services;
    using System;
    using ViewModels.Security;

    public class SecurityCodeHandler :
         IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>,
         IRequestHandler<UpdateSecurityCodeCommand, Result>
    {
        private ISecurityCodeSession securityCodeSession;

        private readonly SecurityCodeService securityCodeService;

        private string PH
        {
            get
            {
                return System.Web.HttpContext.Current.Session["ph"] + string.Empty;
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

        public Result Handle(UpdateSecurityCodeCommand message)
        {
            return new Result();
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