namespace Alpha.Main.Helpers
{
    using Alpha.Main.Configurations;

    public static class MicroSites
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string OldMainUrl(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string HowToUseUrl(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.HowToUseAppPath, virtualPath);
        }
    }
}