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
        private static readonly string StaticResourcesVersion;
        [SuppressMessage("Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate", Justification = "Reviewed!")]
        private static readonly string MinText = ".min";

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed!")]
        static StaticResources()
        {
            if (IsDebugMode())
            {
                MinText = string.Empty;
            }

            StaticResourcesVersion = GetBuildVersion();
        }

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
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed!")]
        public static class AssetScripts
        {
            public static IHtmlString Render(params string[] paths)
            {
                return System.Web.Optimization.Scripts.Render(paths.Select(path => $"{AppSettings.AssetAppPath}{path}{MinText}.js?v={StaticResourcesVersion}").ToArray());
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

        #endregion
    }
}