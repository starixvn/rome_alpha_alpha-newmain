namespace Alpha.Main.Site.Services
{
    using Fanex.Common;
    using System.Configuration;

    public static class AppSettings
    {
        public static string ContentAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ContentAppPath"];
            }
        }

        public static string OldMainAppPath
        {
            get
            {
                return ConfigurationManager.AppSettings["OldMainAppPath"];
            }
        }

        public static int CustIdFoobet
        {
            get
            {
                return Converter.ToInt(ConfigurationManager.AppSettings["CustIdFoobet"]);
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

        public static string GetAppName(string name)
        {
            // Argument.Requires(!string.IsNullOrWhiteSpace(name), nameof(name));   
            return ConfigurationManager.AppSettings[name];
        }
    }
}
