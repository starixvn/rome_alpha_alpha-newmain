namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Linq;
    using System.Web;
    using Alpha.Main.AppResources;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Web.Helpers;
    using Alpha.Web.Models;
    using Configurations;
    using Helpers;
    using Main.Helpers;
    using Models.Requests;
    using SecurityCode.Web;
    using Sunplus.Agent.Utilities;

    public class IndexViewModel
    {
        private readonly IndexQuery message;
        private readonly Func<string> getDefaultUrl;
        private readonly Func<string, string> encodeDataFunction;
        private readonly BrowserHelper browserHelper;
        private readonly ActorProfilePictureModel profilePicture;
        public IndexViewModel(
            IndexQuery message,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction,
            BrowserHelper browserHelper,
            ActorProfilePictureModel profilePicture)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            this.message = message;
            PasswordHash = Encryption.RandomString(4);
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction;
            this.browserHelper = browserHelper;
            this.RequiredNetworkSpeedCalculation = message.CustId > 0 && AppSettings.TrackingConnectionCustIds.Contains(message.CustId);
            DisableMessages = message.DisableMessages;
            this.profilePicture = profilePicture;
            this.ShowOldCustomerList = AppSettings.ShowOldCustomerList;
            IsProtectedMode = message.IsProtectedMode;
            AssetAppPath = message.AssetAppPath;
        }

        public bool EnableSecurityCode => message.EnableSecurityCode;

        public int CustId => message.CustId;

        public int SubAccId => message.SubAccId;

        public int OnUserId => message.OnUserId;

        public string UserLevel => Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLower();

        public string Language => message.Language;

        public string LanguageKey => message.LanguageKey;

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string HowToUseRootPath => UrlHelper.MapHowToUsePath("~/");

        public string MemberInfoRootPath => UrlHelper.MapMemberInfoPath("~/");

        public string AccountInfoRootPath => UrlHelper.MapAccountInfoPath("~/");

        public string ExTotalBetsForecastRootPath => UrlHelper.MapExTotalBetsForecastPath("~/");

        public string MessageSiteRootPath => UrlHelper.MapMessagesPath("~/");

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string PasswordHash { get; }

        public string SubPageAccess => message.SubPageAccess;

        public bool IsSubAccount => message.IsSubAccount;

        public bool IsInternalWebVPN => message.IsInternalWebVpn;

        public bool IsInternal => message.IsInternal || message.IsIndirectLoggedIn;

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, SubAccId, encodeDataFunction);

        public bool IsShowIntroduction => true;

        public bool ShowOldCustomerList { get; }

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public HeaderNavViewModel HeaderNavViewModel =>
            new HeaderNavViewModel(
                CustId,
                message.UserName,
                BuildNickName(message),
                EnableSecurityCode,
                message.ShowSearchAccount,
                IsEnabledOtp,
                string.IsNullOrEmpty(profilePicture.ImageBase64String),
                profilePicture.ImageBase64String,
                message.ShowNicknameMenu,
                message.ShowCreateMemberByQRCode,
                !string.IsNullOrWhiteSpace(message.DarkModeName),
                IsProtectedMode,
                message.IsOfferProtectedMode);

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

        public bool RequiredNetworkSpeedCalculation { get; }

        public bool DisableMessages { get; }

        public bool IsProtectedMode { get; }

        internal string BuildNickName(IndexQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"<p>{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}</p><p>{HttpUtility.HtmlEncode(message.NickName)}</p>";
            }

            if (message.IsInternal)
            {
                return $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}";
            }

            return HttpUtility.HtmlEncode(message.NickName);
        }

        public bool IsOfferLiveChat { get; } = AppSettings.IsOfferLiveChat;

        public int UMAnnouncementPopupClientIntervalInMinutes { get; } = AppSettings.UMAnnouncementPopupClientIntervalInMinutes;

        public string ReportDomain { get; } = AppSettings.ReportDomain;

        public bool IsDarkMode => !string.IsNullOrWhiteSpace(message.DarkModeName);

        public string AssetAppPath { get; }

        public bool IsEnabledCDNReCheckin
            => !string.IsNullOrWhiteSpace(AssetAppPath)
            && !Web.Configurations.AppSettings.AssetAppPath.Equals(AssetAppPath, StringComparison.OrdinalIgnoreCase);

        public int CDNReCheckinIntervalInSeconds { get; } = AppSettings.CDNReCheckinIntervalInSeconds;

        public int RecheckinIntervalMiliseconds => AppSettings.RecheckinIntervalMiliseconds;

        public string SessionID => HttpContext.Current?.Session?.SessionID;

        public string Username => message.IsSubAcc ? message.SubUserName : message.UserName;
        public bool IsShowSuggestEnableTelegramOtp { get; set; }
    }
}
