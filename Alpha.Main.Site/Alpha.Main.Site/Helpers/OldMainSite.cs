namespace Alpha.Main.Site.Helpers
{
    using Alpha.Main.Site.Configurations;

    public static class OldMainSite
    {
        public static string Url(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }
    }
}