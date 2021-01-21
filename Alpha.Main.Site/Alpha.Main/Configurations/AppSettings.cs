namespace Alpha.Main.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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

        public static string AssetAppPath
        {
            get
            {
                return Settings["AssetAppPath"];
            }
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
                return Converter.ToString2(Settings["TotalBetsForecast:AllowedSitesToViewTBF"]);
            }
        }

        public static List<string> DevicesUseCustomScrollbar
        {
            get
            {
                return devicesUseCustomScrollbar;
            }
        }

        public static string StoreProcedurePath => Settings["StoredProcedurePath"];

        public static string MessageSuper => Settings["Message:Super"];

        public static string MessageMaster => Settings["Message:Master"];

        public static string MessageAgent => Settings["Message:Agent"];

        public static bool IsDisabledContactUsLink => Settings["IsDisabledContactUsLink"] == "1";

        public static bool EnablePlayForFun => Settings["EnablePlayForFun"] == "true";

        public static string GoogleAnalyticId => Settings["GoogleAnalyticId"];

        public static bool DisableMessages => Settings["DisableMessages"] == "1";

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MBC", Justification = "Reviewed!")]
        public static bool IsMBCSite => !string.IsNullOrWhiteSpace(Settings["SiteGroup"])
            && !Settings["SiteGroup"].Equals("789y", StringComparison.OrdinalIgnoreCase);

        public static bool DisabledVietnamese => IsMBCSite;

        public static bool ShowGoldDeluxeAnnouncement => IsMBCSite;

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
        public static string ChatBotBaseUrl => Settings["Chatbot:BaseUrl"];

        public static string ChatBotApiKey => Settings["Chatbot:ApiKey"];

        public static int IntervalThresholdToSendRequestInSeconds
            => int.Parse(Settings["IntervalThresholdToSendRequestInSeconds"] ?? "2");

        public static string[] OfferChatbotSites
            => Settings["OfferChatbotSites"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public static string CreatedSettingLastDateSessionKey => "CreatedSettingLastDate";

        public static bool BannerIsHide => "1" == Settings["Banner:IsHide"];

        public static string BannerEnvironment => Settings["Banner:Environment"];

        public static string SiteGroup => Settings["SiteGroup"];

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
    }
}