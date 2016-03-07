namespace Alpha.Main.Site.App_Start
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterJavascriptBundles(bundles);
            RegisterStyleBundles(bundles);
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            RegisterMainJavascript(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            RegisterMainStyle(bundles);
        }

        private static void RegisterMainJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/main").Include(
                "~/Views/Shared/Scripts/SharedScripts.js",
                "~/Views/CustomerProfile/Scripts/ViewProfile.js",
                "~/Views/Main/Scripts/Core.js",
                "~/Views/Main/Scripts/Header.js",
                "~/Views/Main/Scripts/Dashboard.js",
                "~/Views/Announcement/Scripts/AnnouncementManager.js"));
        }

        private static void RegisterMainStyle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundle/styles/main")
                .Include("~/Content/Main/Styles/sass/page.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Main/Styles/sass/Dashboard.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Main/Styles/sass/PopupMessage.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/CustomerProfile/Styles/css/CustomerProfile.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Announcement/Styles/css/Announcements.css", new CssRewriteUrlTransformWrapper()));
        }
    }
}