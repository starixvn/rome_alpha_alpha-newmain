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
                return ConfigurationManager.AppSettings["MessageAppPath"];
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

        public static string GetMessagesDataPath => Settings["MessagesDataPath"];

        public static string MessageSuper => Settings["Message:Super"];

        public static string MessageMaster => Settings["Message:Master"];

        public static string MessageAgent => Settings["Message:Agent"];

        public static bool IsDisabledContactUsLink => Settings["IsDisabledContactUsLink"] == "1";

        public static bool EnablePlayForFun => Settings["EnablePlayForFun"] == "true";

        public static string GoogleAnalyticId => Settings["GoogleAnalyticId"];

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MBC", Justification = "Reviewed!")]
        public static bool IsMBCSite => !string.IsNullOrWhiteSpace(Settings["SiteGroup"])
            && Settings["SiteGroup"] == "IBCBET";

        public static bool DisabledVietnamese => IsMBCSite;

        public static bool ShowGoldDeluxeAnnouncement => IsMBCSite;

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
    }
}
