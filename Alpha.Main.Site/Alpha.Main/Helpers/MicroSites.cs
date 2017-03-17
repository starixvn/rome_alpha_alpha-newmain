namespace Alpha.Main.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Configurations;

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
        public static string ReportSiteUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.ReportAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MemberInfoSiteUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.MemberInfoAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MainUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, virtualPath);
        }
    }
}