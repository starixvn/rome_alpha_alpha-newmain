namespace Alpha.Main.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Alpha.Main.Configurations;

    public static class MicroSites
    {
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string OldMainUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string HowToUseUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.HowToUseAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MainUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, virtualPath);
        }
    }
}