namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using MediatR;
    using Sessions;
    using System.Configuration;
    using ViewModels.Security;
    public class SelectSecurityCodeQuery : IRequest<SecurityCodeViewModel>
    {
        private ISecurityCodeSession securityCodeSession;
        public SelectSecurityCodeQuery(ISecurityCodeSession securityCodeSession)
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