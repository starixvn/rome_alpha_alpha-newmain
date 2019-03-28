namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Linq;
    using System.Web;
    using Alpha.Main.AppResources;
    using Alpha.Main.Models.CustomerProfiles;
    using Configurations;
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
        private readonly ProfilePicture profilePicture;
        public IndexViewModel(
            IndexQuery message,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction,
            BrowserHelper browserHelper,
            ProfilePicture profilePicture)
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

        public bool IsInternal => message.IsInternal;

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, SubAccId, encodeDataFunction);

        public bool IsShowIntroduction => true;

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public HeaderNavViewModel HeaderNavViewModel => 
            new HeaderNavViewModel(
                CustId,
                message.UserName,
                BuildNickName(message),
                EnableSecurityCode,
                message.IsViewedMemberInfo,
                IsEnabledOtp,
                profilePicture.UseDefaultAvatar,
                profilePicture.ImageData != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.ImageData)}" : string.Empty);

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

        internal string BuildNickName(IndexQuery message)
            => message.IsSubAcc ? $"{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}" :
               message.IsInternal ? $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}" :
               HttpUtility.HtmlEncode(message.NickName);
    }
}
