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
            RegisterHomeJavascript(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            RegisterMainStyle(bundles);
            RegisterHomeStyle(bundles);
        }

        private static void RegisterHomeJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/home").Include(
                "~/Content/Shared/Scripts/SharedScripts.js",
                "~/Content/Home/Scripts/Core.js",
                "~/Content/Home/Scripts/Header.js",
                "~/Content/Home/Scripts/IndexPage.js",
                "~/Content/Announcement/Scripts/AnnouncementManager.js"));
        }

        private static void RegisterHomeStyle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundle/styles/home")
                .Include("~/Content/Home/Styles/sass/page.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Home/Styles/sass/Dashboard.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Home/Styles/sass/PopupMessage.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/CustomerProfile/Styles/css/CustomerProfile.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Announcement/Styles/css/Announcements.css", new CssRewriteUrlTransformWrapper()));
        }

        private static void RegisterMainJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/main").Include(
                "~/Content/Shared/Scripts/SharedScripts.js",
                "~/Content/CustomerProfile/Scripts/ViewProfile.js",
                "~/Content/Main/Scripts/Core.js",
                "~/Content/Main/Scripts/Header.js",
                "~/Content/Main/Scripts/SideBar.js",
                "~/Content/Main/Scripts/IndexPage.js",
                "~/Content/Announcement/Scripts/AnnouncementManager.js"));
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