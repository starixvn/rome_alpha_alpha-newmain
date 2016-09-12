namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using MediatR;
    using Sessions;
    using System.Configuration;
    using ViewModels.Security;
    public class SecurityCodeQuery : IRequest<SecurityCodeViewModel>
    {
        private ISecurityCodeSession securityCodeSession;
        public SecurityCodeQuery(ISecurityCodeSession securityCodeSession)
        {
            this.securityCodeSession = securityCodeSession;
        }

        public string Language
        {
            get
            {
                return this.securityCodeSession.Language;
            }
        }

        public string VirtualDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["VirtualDirectory"];
            }
        }
    }
}