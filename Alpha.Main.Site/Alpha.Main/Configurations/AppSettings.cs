namespace Alpha.Main.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.Common;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection settings;

        private static List<string> devicesUseCustomScrollbar = Converter.ToString2(Settings["DevicesUseCustomScrollbar"]).Split(',').ToList();

        internal static NameValueCollection Settings
        {
            get { return settings ?? ConfigurationManager.AppSettings; }

            set { settings = value; }
        }

        public static string OldMainAppPath
        {
            get
            {
                return Settings["OldMainAppPath"];
            }
        }

        public static string HowToUseAppPath
        {
            get
            {
                return Settings["HowToUseAppPath"];
            }
        }

        public static string ReportAppPath
        {
            get
            {
                return Settings["ReportsAppPath"];
            }
        }

        public static string ExTotalBetsForecastAppPath
        {
            get
            {
                return Settings["ExTotalBetsForecastAppPath"];
            }
        }

        public static string MessageAppPath
        {
            get
            {
                return Settings["MessageAppPath"];
            }
        }

        public static string MemberInfoAppPath
        {
            get
            {
                return Settings["MemberInfoAppPath"];
            }
        }

        public static string UsersAlwaysAllowAccessBetsListMonitoring
        {
            get
            {
                return Converter.ToString2(Settings["UsersAlwaysAllowAccessBetsListMonitoring"]);
            }
        }

        public static bool EnableBetsListMonitoring
        {
            get
            {
                return Converter.ToBool(Settings["EnableBetsListMonitoring"]);
            }
        }

        public static string VirtualDirectory
        {
            get
            {
                return Converter.ToString2(Settings["VirtualDirectory"]);
            }
        }

        public static string NonUseSecurityCodeSites
        {
            get
            {
                return Converter.ToString2(Settings["NonUseSecurityCodeSites"]);
            }
        }

        public static string StaticResourcesVersion
        {
            get
            {
                return Converter.ToString2(Settings["AssetsApp:StaticResourcesVersion"]);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TBF", Justification = "Reviewed!")]
        public static string AllowedSitesToViewTBF
        {
            get
            {
                return Converter.ToString2(Settings["NewTBF:AllowSitesToViewForecast"]);
            }
        }

        public static string[] NewTBFAllowedSites
        {
            get => (AllowedSitesToViewTBF ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static List<string> DevicesUseCustomScrollbar
        {
            get
            {
                return devicesUseCustomScrollbar;
            }
        }

        public static string StoreProcedurePath => Settings["StoredProcedurePath"];

        public static string FanexDataServiceApi => Settings["FanexDataServiceApi"];

        public static string MessageSuper => Settings["Message:Super"];

        public static string MessageMaster => Settings["Message:Master"];

        public static string MessageAgent => Settings["Message:Agent"];

        public static bool IsDisabledContactUsLink => Settings["IsDisabledContactUsLink"] == "1";

        public static string GoogleAnalyticId => Settings["GoogleAnalyticId"];

        public static bool DisableMessages => Settings["DisableMessages"] == "1";

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MBC", Justification = "Reviewed!")]
        public static bool IsMBCSite => !IsSiteLabel789y;

        public static bool IsSiteLabel789y => SiteLabel.Equals(SiteLabel789y);

        public static bool DisabledVietnamese => IsMBCSite;

        public static bool ShowHelpLink => IsMBCSite;

        public static string[] NoticeTypes
        {
            get
            {
                var splitChars = new[] { ',' };

                return Settings["NoticeTypes"].Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static string GetAppPath(string name)
        {
            return Settings[name];
        }

        public static bool LocalTest => Settings["LocalTest"] == "1";

        public static IEnumerable<int> TrackingConnectionCustIds { get; }
            = string.IsNullOrEmpty(Settings["TrackingConnectionCustIds"])
                ? Enumerable.Empty<int>()
                : Settings["TrackingConnectionCustIds"]
                    .Split(',')
                    .Where(custIdString => !string.IsNullOrWhiteSpace(custIdString))
                    .Select(custIdString => Converter.ToInt(custIdString));

        public static string RemoteRefURL => Settings["RemoteRefURL"];

        public static string CreatedSettingLastDateSessionKey => "CreatedSettingLastDate";

        public static bool BannerIsHide => "1" == Settings["Banner:IsHide"];

        public static string BannerEnvironment => Settings["Banner:Environment"];

        public static bool BannerIsHideMobileSabaApp => "1" == Settings["Banner:IsHideMobileSabaApp"];

        public static string SiteLabel => Settings["SiteLabel"];

        public static string SiteLabel789y => Settings["SiteLabel789y"];

        public static IEnumerable<int> IndirectLoginSuperIds
            => Settings["IndirectLogin:AllowSuperIds"]
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select
            (
                id => string.IsNullOrWhiteSpace(id) ? 0 : ValueOfNotNullOrWhiteSpace(id)
            );

        public static bool IsAllowedIndirectLogin(int srecommend)
            => IndirectLoginSuperIds.Contains(srecommend) || IndirectLoginSuperIds.Any(id => id == int.MaxValue);

        public static bool EnableUMAnnouncementPopup => "1" == Settings["EnableUMAnnouncementPopup"];

        public static int UMAnnouncementPopupServerCacheInMinutes
            => int.Parse(Settings["UMAnnouncementPopup:ServerCacheInMinutes"]);

        public static int UMAnnouncementPopupClientIntervalInMinutes
            => int.Parse(Settings["UMAnnouncementPopup:ClientIntervalInMinutes"]);

        private static string[] allowedSitesToViewBonusDetail;

        public static string[] AllowedSitesToViewBonusDetail
        {
            get
            {
                if (allowedSitesToViewBonusDetail == null)
                {
                    var allowedSites = Settings["AllowedSitesToViewBonusDetail"];

                    allowedSitesToViewBonusDetail = string.IsNullOrWhiteSpace(allowedSites)
                        ? Array.Empty<string>()
                        : allowedSites
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim().ToUpperInvariant())
                            .ToArray();
                }

                return allowedSitesToViewBonusDetail;
            }
        }

        private static int ValueOfNotNullOrWhiteSpace(string value)
            => value.Equals("*", StringComparison.OrdinalIgnoreCase) ? int.MaxValue : Convert.ToInt32(value);

        public static IEnumerable<string> NewReportsOfferSites
            => Settings["NewReports.OfferSites"]
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(site => site.Trim());

        public static IEnumerable<int> NewReportsIgnoreSuperIds
            => Settings["NewReports.IgnoreSuperIds"]
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select
            (
                id => string.IsNullOrWhiteSpace(id) ? 0 : ValueOfNotNullOrWhiteSpace(id.Trim())
            );

        public static bool IsUsedNewReports(string site, int srecommend)
            => (NewReportsOfferSites.Contains("*", StringComparer.CurrentCultureIgnoreCase)
            || NewReportsOfferSites.Contains(site, StringComparer.CurrentCultureIgnoreCase))
            && !NewReportsIgnoreSuperIds.Any(id => id == int.MaxValue)
            && !NewReportsIgnoreSuperIds.Any(id => id == srecommend);

        public static bool IsUsedOldWinLossDetailReports(int srecommend)
        {
            var offerSites = Settings["OfferOldWinLossDetail:SuperIds"] ?? string.Empty;
            return offerSites == "*" ||
                offerSites.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains(srecommend.ToString());
        }

        public static bool ShowOldCustomerList => Settings["ShowOldCustomerList"] == "1";

        public static IEnumerable<string> NewCaptchaLogCurrencies => string.IsNullOrWhiteSpace(Settings["NewCaptcha:LogCurrencies"])
            ? Enumerable.Empty<string>()
            : Settings["NewCaptcha:LogCurrencies"].Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

        public static IEnumerable<string> TransferToDownlineHistoryOfferSites
            => Settings["TransferToDownlineHistory.OfferSites"]
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(site => site.Trim());

        public static bool IsAllowedTransferToDownlineHistorySite(string site)
            => TransferToDownlineHistoryOfferSites.Contains("*", StringComparer.CurrentCultureIgnoreCase)
            || TransferToDownlineHistoryOfferSites.Contains(site, StringComparer.CurrentCultureIgnoreCase);

        public static bool IsShowEmptyDashboard => "1" == Settings["IsShowEmptyDashboard"];

        public static string[] DomainsWithViva88ContactInfo =>
            (string.Empty + Settings["newdomains"]).Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

        public static bool IsViva88Domain
            => DomainsWithViva88ContactInfo.Any(domain => domain.Equals(HttpContext.Current.Request.Headers["Host"], StringComparison.OrdinalIgnoreCase));

        public static string LiveChatLicenseID => Settings["LiveChatLicenseID"];

        public static string ReportDomain
        {
            get
            {
                var reportDomain = string.Empty + Settings["Report:Domain"];
                var mainDomain = HttpContext.Current?.Request?.Headers?["Host"];

                if (HttpContext.Current?.Session?.IsCookieless == true)
                {
                    mainDomain = mainDomain?.Replace($"{mainDomain?.Split('.')?.ElementAt(0)}.", string.Empty);
                }

                reportDomain = reportDomain.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) != -1 ?
                    string.Format(reportDomain, mainDomain) : reportDomain;

                return reportDomain;
            }
        }

        public static string BetlistDomain
        {
            get
            {
                var betlistDomain = string.Empty + Settings["Betlist:Domain"];
                var mainDomain = HttpContext.Current?.Request?.Headers?["Host"];

                if (HttpContext.Current?.Session?.IsCookieless == true)
                {
                    mainDomain = mainDomain?.Replace($"{mainDomain?.Split('.')?.ElementAt(0)}.", string.Empty);
                }

                betlistDomain = betlistDomain.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) != -1 ?
                    string.Format(betlistDomain, mainDomain) : betlistDomain;

                return betlistDomain;
            }
        }

        public static string ReportRouting => Settings["Report:Routing"] ?? string.Empty;

        public static bool IsStandaloneReports => !string.IsNullOrWhiteSpace(Settings["Report:Domain"]);

        public static bool IsStandaloneBetList => !string.IsNullOrWhiteSpace(Settings["Betlist:Domain"]);

        public static string ContactUsSkypeId => Settings["skypeId"];

        public static string ContactUsLetstalkId => Settings["letstalkid"];

        public static string ContactUsLetstalkIdNew => Settings["letstalkidNew"];

        public static string ContactUsLetstalkUrl => Settings["letstalkurl"];

        public static string ContactUsLetstalkUrlNew => Settings["letstalkurlNew"];

        public static string ContactUsNewDomains => Settings["newdomains"];

        public static Uri TelegramBotClientUri => new Uri(Settings["TelegramBotClientUri"]);

        public static bool EnabledUMTelegramChatbot => Settings["TelegramBot:EnabledUM"] == "1";

        public static string TelegramChatbotUMFrom => Settings["TelegramBot:UMFrom"];

        public static string TelegramChatbotUMTo => Settings["TelegramBot:UMTo"];

        public static int CDNReCheckinIntervalInSeconds
            => int.Parse(Settings["CDNReCheckin:IntervalInSeconds"] ?? "60");

        public static int RecheckinIntervalMiliseconds
            => string.IsNullOrWhiteSpace(Settings["Recheckin:IntervalMiliseconds"]) ? 30000 :
                int.Parse(Settings["Recheckin:IntervalMiliseconds"]);

        public static string JsVersion => Settings["JsVersion"];

        public static bool EnableLogDifferentSessionBetweenMainAndReports { get; } = "1" == Settings["EnableLogDifferentSessionBetweenMainAndReports"];

        public static bool IsEnabledRechekin => Settings["DisabledCheckDuplicationLogin"] == "0";
        public static Uri ApiKeyServiceUri => new Uri(Settings["ApiKeyServiceUri"]);
        public static bool IsStoreUserLangEnabled => Settings["IsStoreUserLangEnabled"] == "1";

        public static string APIGatewaysURL => Settings["APIGatewaysURL"];
        public static List<string> APISuperIds => Settings["APISuperIds"].Split(',').ToList();

        public static bool IsOfferCreateAccountLiteMode => Settings["CreateNewAccount:OfferLiteMode"] == "1";

        public static bool SupportedNewHandbook
        {
            get
            {
                var marketsSupportedNewHandbook = Settings["MarketsSupportedNewHandbook"];
                if (string.IsNullOrWhiteSpace(marketsSupportedNewHandbook))
                {
                    return false;
                }

                if (string.Equals("*", marketsSupportedNewHandbook, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var label = SiteLabel;
                return marketsSupportedNewHandbook
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(s => string.Equals(s, label, StringComparison.OrdinalIgnoreCase));
            }
        }

        private static HashSet<string> dashboardBannerOfferedSites;

        public static HashSet<string> DashboardBannerOfferedSites
        {
            get
            {
                var config = Settings["Dashboard:BannerOfferedSites"] ?? string.Empty;
                dashboardBannerOfferedSites = dashboardBannerOfferedSites ?? new HashSet<string>(
                    config.Split(',').Select(s => s.Trim()),
                    StringComparer.OrdinalIgnoreCase);

                return dashboardBannerOfferedSites;
            }
        }

        public static bool IsOfferedWhiteLabel(string site, string level)
            => (WhiteLabelOfferedSites.Contains("*") || WhiteLabelOfferedSites.Contains(site))
            && DateTime.Parse(Settings[$"WhiteLabel:{level}:OfferedDate"]) <= DateTime.Today;

        private static IEnumerable<string> WhiteLabelOfferedSites { get; } = new HashSet<string>(
            (Settings["WhiteLabel:OfferedSites"] ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(site => site.Trim()), StringComparer.OrdinalIgnoreCase);
        public static bool IsOfferMemberBettingBehavior { get; } = Settings["IsOfferMemberBettingBehavior"] == "1";
        
        public static Uri WhiteLabelBackOfficeApiUri => new Uri(Settings["WhiteLabel:BackOfficeApiUrl"]);

        public static string WhiteLabelProxyUrl => Settings["WhiteLabel:ProxyUrl"] ?? string.Empty;
        
    }
}