namespace Alpha.Main.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;

    public static class AppSettings
    {
        private static List<string> devicesUseCustomScrollbar = Converter.ToString2(ConfigurationManager.AppSettings["DevicesUseCustomScrollbar"]).Split(',').ToList();

        private static string googleAnalyticId = ConfigurationManager.AppSettings["GoogleAnalyticId"];

        private static bool isMBCSite = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SiteGroup"])
                && ConfigurationManager.AppSettings["SiteGroup"] == "IBCBET";

        public static string AssetAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["AssetAppPath"];
            }
        }

        public static string OldMainAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["OldMainAppPath"];
            }
        }

        public static string HowToUseAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["HowToUseAppPath"];
            }
        }

        public static string ReportAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsAppPath"];
            }
        }

        public static string MemberInfoAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["MemberInfoAppPath"];
            }
        }

        public static string UsersAlwaysAllowAccessBetsListMonitoring
        {
            get
            {
                return Converter.ToString2(ConfigurationManager.AppSettings["UsersAlwaysAllowAccessBetsListMonitoring"]);
            }
        }

        public static bool EnableBetsListMonitoring
        {
            get
            {
                return Converter.ToBool(ConfigurationManager.AppSettings["EnableBetsListMonitoring"]);
            }
        }

        public static string VirtualDirectory
        {
            get
            {
                return Converter.ToString2(ConfigurationManager.AppSettings["VirtualDirectory"]);
            }
        }

        public static string NonUseSecurityCodeSites
        {
            get
            {
                return Converter.ToString2(ConfigurationManager.AppSettings["NonUseSecurityCodeSites"]);
            }
        }

        public static string StaticResourcesVersion
        {
            get
            {
                return Converter.ToString2(ConfigurationManager.AppSettings["AssetsApp:StaticResourcesVersion"]);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TBF", Justification = "Reviewed!")]
        public static string AllowedSitesToViewTBF
        {
            get
            {
                return Converter.ToString2(ConfigurationManager.AppSettings["TotalBetsForecast:AllowedSitesToViewTBF"]);
            }
        }

        public static List<string> DevicesUseCustomScrollbar
        {
            get
            {
                return devicesUseCustomScrollbar;
            }
        }

        public static string StoreProcedurePath => ConfigurationManager.AppSettings["StoreProcedurePath"];

        public static string GetMessagesDataPath => ConfigurationManager.AppSettings["MessagesDataPath"];

        public static string MessageSuper => ConfigurationManager.AppSettings["Message:Super"];

        public static string MessageMaster => ConfigurationManager.AppSettings["Message:Master"];

        public static string MessageAgent => ConfigurationManager.AppSettings["Message:Agent"];

        public static bool IsDisabledContactUsLink => ConfigurationManager.AppSettings["IsDisabledContactUsLink"] == "1";

        public static bool EnablePlayForFun => ConfigurationManager.AppSettings["EnablePlayForFun"] == "true";

        public static string GoogleAnalyticId => googleAnalyticId;

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MBC", Justification = "Reviewed!")]
        public static bool IsMBCSite => isMBCSite;

        public static bool DisabledVietnamese => IsMBCSite;

        public static string GetAppPath(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}