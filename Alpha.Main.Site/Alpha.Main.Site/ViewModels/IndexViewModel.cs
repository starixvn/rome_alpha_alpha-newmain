namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using System.Web;
    using Helpers;
    using Main.Helpers;
    using Main.Models.Enumerations;
    using Models.Requests;
    using SecurityCode.Web;
    using Sunplus.Agent.Utilities;

    public class IndexViewModel
    {
        private readonly IndexQuery message;
        private readonly Func<string> getDefaultUrl;
        private readonly Func<string, string> encodeDataFunction;
        private readonly BrowserHelper browserHelper;

        public IndexViewModel(
            IndexQuery message,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction,
            BrowserHelper browserHelper)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            this.message = message;
            PasswordHash = Encryption.RandomString(new Random().Next(4, 8));
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction;
            this.browserHelper = browserHelper;
        }

        public bool EnableSecurityCode => message.EnableSecurityCode;

        public int CustId => message.CustId;

        public int SubAccId => message.SubAccId;

        public int OnUserId => message.OnUserId;

        public string UserLevel => Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLower();

        public string Language => message.Language;

        public string LanguageKey => message.LanguageKey;

        public string OldMainRootPath => MicroSites.OldMainPath("~/");

        public string HowToUseRootPath => MicroSites.HowToUsePath("~/");

        public string ExTotalBetsForecastRootPath => MicroSites.ExTotalBetsForecastSitePath();        

        public string MessageSiteRootPath => MicroSites.MessageSiteUrl();

        public string MainRootPath => AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");

        public string PasswordHash { get; }

        public string SubPageAccess => message.SubPageAccess;

        public bool IsSubAccount => message.IsSubAccount;

        public bool IsInternalWebVPN => message.IsInternalWebVpn;

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed!")]
        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, SubAccId, encodeDataFunction);

        public bool IsShowIntroduction => true;

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public HeaderNavViewModel HeaderNavViewModel => new HeaderNavViewModel(CustId, EnableSecurityCode, message.IsViewedMemberInfo);

        public bool IsEnabledOtp => SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp;

        public double InitialScaleViewPort
        {
            get
            {
                var browserName = browserHelper.GetDevice().Info.BrowserName.ToLower();
                var userAgent = browserHelper.GetUserAgent().ToLower();
                var isTablet = browserHelper.IsMobile(isOnlyTablet: true);

                if (browserName.Contains("chrome") && userAgent.Contains("android") && !browserHelper.IsAndroidNativeBrowser(userAgent))
                {
                    if (!isTablet)
                    {
                        return 0.5;
                    }
                }

                return 0.7;
            }
        }
    }
}