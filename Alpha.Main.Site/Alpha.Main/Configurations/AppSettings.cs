namespace Alpha.Main.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;

    public static class AppSettings
    {
        private static List<string> devicesUseCustomScrollbar;

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
        
        public static string SiteMainAppPath
        {
            get
            {
                return "/" + ConfigurationManager.AppSettings["AppMainName"];
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
                return Converter.ToString2(ConfigurationManager.AppSettings["ContentApp:StaticResourcesVersion"]);
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
                devicesUseCustomScrollbar =
                    devicesUseCustomScrollbar ??
                    Converter.ToString2(ConfigurationManager.AppSettings["DevicesUseCustomScrollbar"]).Split(',').ToList();

                return devicesUseCustomScrollbar;
            }
        }

        public static string GetAppPath(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}