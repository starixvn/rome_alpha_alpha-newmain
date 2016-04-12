namespace Alpha.Main.Helpers
{
    using Alpha.Main.Configurations;

    public static class MicroSites
    {
        public static string OldMainUrl(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }

        public static string HowToUseUrl(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.HowToUseAppPath, virtualPath);
        }
    }
}