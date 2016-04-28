namespace Alpha.Main.Site.Helpers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using Configurations;
    using Main.Helpers;

    public static class ContentSite
    {
        private static readonly string StaticResourcesVersion;

        static ContentSite()
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            StaticResourcesVersion = fvi.FileVersion;
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