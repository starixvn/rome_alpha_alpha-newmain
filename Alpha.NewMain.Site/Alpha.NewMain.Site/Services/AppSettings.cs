namespace Alpha.NewMain.Site.Services
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string ContentSite
        {
            get
            {
                return ConfigurationManager.AppSettings["ContentSite"];
            }
        }

        public static string MainSite
        {
            get
            {
                return ConfigurationManager.AppSettings["MainSite"];
            }
        }
    }
}
