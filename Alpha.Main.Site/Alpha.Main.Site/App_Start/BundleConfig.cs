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
                "~/Content/Scripts/core.js",
                "~/Content/Scripts/Home/header-nav.js",
                "~/Content/Scripts/Home/header-message.js",
                "~/Content/Scripts/Home/menu-bar.js",
                "~/Content/Scripts/CustomerProfiles/brief-profile.js",
                "~/Content/Scripts/Home/index-page.js",
                "~/Content/Scripts/Announcement/announcement.js"));
        }

        private static void RegisterHomeStyle(BundleCollection bundles)
        {
            var cssRewriteUrlTransformWrapper = new CssRewriteUrlTransformWrapper();
            bundles.Add(new StyleBundle("~/bundle/styles/home")
                .Include("~/Content/Styles/Home/page.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/Styles/Home/PopupMessage.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/Styles/CustomerProfiles/CustomerProfile.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/Styles/Announcement/Announcements.css", cssRewriteUrlTransformWrapper));
        }
    }
}