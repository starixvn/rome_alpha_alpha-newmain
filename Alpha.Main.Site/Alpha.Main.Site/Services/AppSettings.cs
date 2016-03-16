namespace Alpha.Main.Site.Services
{
    using System.Configuration;
    using Fanex.Common;

    public static class AppSettings
    {
        public static string ContentSite
        {
            get
            {
                return ConfigurationManager.AppSettings["ContentSite"];
            }
        }

        public static string OldMainApp
        {
            get
            {
                return ConfigurationManager.AppSettings["OldMainApp"];
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
    }
}
