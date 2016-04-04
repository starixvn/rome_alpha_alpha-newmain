namespace Alpha.Main.Site.Helpers
{
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.SessionState;
    using Configurations;

    public static class AppPathHelper
    {
        public static string GetAppPath(string appName, string virtualPath)
        {
            Argument.Requires(!string.IsNullOrWhiteSpace(appName), nameof(appName));
            Argument.Requires(!string.IsNullOrWhiteSpace(virtualPath), nameof(virtualPath));

            if(HttpContext.Current == null || HttpContext.Current.Handler == null)
            {
                return string.Empty;
            }

            var appPath = HttpContext.Current.Response.ApplyAppPathModifier(virtualPath);
            appPath = Regex.Replace(appPath, HttpRuntime.AppDomainAppVirtualPath, appName, RegexOptions.IgnoreCase);
            return appPath;
        }

        public static string GetOldMainPath(string virtualPath)
        {
            return GetAppPath(AppSettings.OldMainAppPath, virtualPath);
        }

        public static string GetContent(string virtualPath)
        {
            return GetAppPath(AppSettings.ContentAppPath, virtualPath);
        }
    }
}