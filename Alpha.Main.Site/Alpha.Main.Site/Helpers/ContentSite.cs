namespace Alpha.Main.Site.Helpers
{
    using System.Linq;
    using System.Web;
    using Configurations;
    using Main.Helpers;

    public static class ContentSite
    {
        private static readonly string StaticResourcesVersion;

        static ContentSite()
        {
            StaticResourcesVersion = AppSettings.StaticResourcesVersion;
        }

        public static string Url(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.ContentAppPath, virtualPath);
        }

        public static class Styles
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Styles.Render(paths.Select(path => $"{AppSettings.ContentAppPath}{path}?v={StaticResourcesVersion}").ToArray());
            }
        }

        public static class Scripts
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Scripts.Render(paths.Select(path => $"{AppSettings.ContentAppPath}{path}?v={StaticResourcesVersion}").ToArray());
            }
        }
    }
}