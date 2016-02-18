namespace Alpha.NewMain.Library
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
    }
}
