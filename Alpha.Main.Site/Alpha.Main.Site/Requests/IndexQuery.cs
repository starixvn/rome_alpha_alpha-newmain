namespace Alpha.Main.Site.Models.Requests
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        private readonly bool showSecurityVerify;
        private readonly IUserSession userSession;
        private readonly ISecurityCodeSession securityCodeSession;

        private readonly bool isPiwikEnable;
        private readonly string piwikUrl;
        private readonly string piwikScript;
        private readonly string piwikSiteId;

        public IndexQuery(
            IUserSession userSession,
            ISecurityCodeSession securityCodeSession,
            bool showSecurityVerify,
            bool isPiwikEnable = false,
            string piwikUrl = "",
            string piwikScript = "",
            string piwikSiteId = "")
        {
            this.userSession = userSession;
            this.securityCodeSession = securityCodeSession;
            this.showSecurityVerify = showSecurityVerify;

            this.isPiwikEnable = isPiwikEnable;
            this.piwikUrl = piwikUrl;
            this.piwikScript = piwikScript;
            this.piwikSiteId = piwikSiteId;
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public int OnUserId
        {
            get
            {
                return userSession.OnUserId;
            }
        }

        public int SubAccId
        {
            get
            {
                return userSession.SubAccId;
            }
        }

        public byte RoleId
        {
            get
            {
                return userSession.RoleId;
            }
        }

        public string Language
        {
            get
            {
                return userSession.Language;
            }
        }

        public string LanguageKey
        {
            get
            {
                return userSession.LangKey;
            }
        }

        public string SubPageAccess
        {
            get
            {
                return userSession.SubPageAccess;
            }
        }

        public bool IsSubAccount
        {
            get
            {
                return userSession.IsSubAcc;
            }
        }

        public bool EnableSecurityCode
        {
            get
            {
                return userSession.IsLogged &&
                    !userSession.IsPasswordExpired &&
                    (securityCodeSession.IsEnableSecurityCode || this.showSecurityVerify);
            }
        }

        public bool IsPiwikEnable
        {
            get
            {
                return isPiwikEnable;
            }
        }

        public string PiwikUrl
        {
            get
            {
                return piwikUrl;
            }
        }

        public string PiwikScript
        {
            get
            {
                return piwikScript;
            }
        }

        public string PiwikSiteId
        {
            get
            {
                return piwikSiteId;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN", Justification = "Reviewed!")]
        public bool IsInternalWebVPN
        {
            get
            {
                return userSession.IsInternalWebVPN;
            }
        }
    }
}