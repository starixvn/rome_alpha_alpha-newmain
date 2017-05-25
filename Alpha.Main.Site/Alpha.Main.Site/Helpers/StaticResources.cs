namespace Alpha.Main.Site.Helpers
{
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    using Configurations;

    public static class StaticResources
    {
        private static readonly string StaticResourcesVersion = GetBuildVersion();

        private static readonly string MinText = IsDebugMode() ? string.Empty : ".min";
              
        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }

        private static bool IsDebugMode()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

            return compilationSection.Debug;
        }

        #region Assets App

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed!")]
        public static class AssetStyles
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Styles.Render(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{MinText}.css?v={StaticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommon(string path, string version = null)
            {
                version = !string.IsNullOrEmpty(version) ? version : AppSettings.StaticResourcesVersion;

                return System.Web.Optimization.Styles.Render($"{AppSettings.AssetAppPath}{path}{MinText}.css?v={version}");
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed!")]
        public static class AssetScripts
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Scripts.Render(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{MinText}.js?v={StaticResourcesVersion}").ToArray());
            }

            public static IHtmlString RenderCommon(string path, string version = null)
            {
                version = !string.IsNullOrEmpty(version) ? version : AppSettings.StaticResourcesVersion;

                return System.Web.Optimization.Scripts.Render($"{AppSettings.AssetAppPath}{path}{MinText}.js?v={version}");
            }
        }

        #endregion Assets App

        #region Current site

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MainSiteScriptUrl(string relativePath)
        {
            return VirtualPathUtility.ToAbsolute($"{relativePath}{MinText}.js?v={StaticResourcesVersion}");
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string MainSiteStyleUrl(string relativePath)
        {
            return VirtualPathUtility.ToAbsolute($"{relativePath}{MinText}.css?v={StaticResourcesVersion}");
        }

        #endregion Current site
    }
}