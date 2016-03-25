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
            RegisterHomeJavascript(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            RegisterHomeStyle(bundles);
        }

        private static void RegisterHomeJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/home").Include(
                "~/Content/Shared/Scripts/SharedScripts.js",
                "~/Content/Home/Scripts/Core.js",
                "~/Content/Home/Scripts/Header.js",
                "~/Content/Home/Scripts/header.navigation.js",
                "~/Content/Home/Scripts/header.notification.js",
                "~/Content/Home/Scripts/SideBar.js",
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
    }
}