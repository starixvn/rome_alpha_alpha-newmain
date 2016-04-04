namespace Alpha.Main.Site.Helpers
{
    using System.Linq;
    using System.Web;
    using Configurations;

    public static class ContentSite
    {
        public static class Styles
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Styles.Render(paths.Select(path => AppSettings.ContentAppPath + path).ToArray());
            }
        }

        public static class Scripts
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Scripts.Render(paths.Select(path => AppSettings.ContentAppPath + path).ToArray());
            }
        }

        public static string Url(string virtualPath)
        {
            return AppPathHelper.GetAppPath(AppSettings.ContentAppPath, virtualPath);
        }
    }
}