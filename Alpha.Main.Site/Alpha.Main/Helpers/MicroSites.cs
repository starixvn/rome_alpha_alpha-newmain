namespace Alpha.Main.Helpers
{
    using Configurations;
    using System.Web;

    public static class MicroSites
    {
        public static string ExTotalBetsForecastSitePath(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.ExTotalBetsForecastAppPath, virtualPath);
        }

        public static string OldMainPath(string virtualPath = "~/")
        public static string MessageSiteUrl(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.MessageAppPath, virtualPath);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        {
            return AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }

        public static string HowToUsePath(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.HowToUseAppPath, virtualPath);
        }

        public static string ReportSitePath(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.ReportAppPath, virtualPath);
        }

        public static string MemberInfoSitePath(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(AppSettings.MemberInfoAppPath, virtualPath);
        }

        public static string MainPath(string virtualPath = "~/")
        {
            return AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, virtualPath);
        }
    }
}
