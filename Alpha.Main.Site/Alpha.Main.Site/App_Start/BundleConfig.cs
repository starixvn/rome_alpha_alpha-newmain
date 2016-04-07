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
            bundles.Add(new ScriptBundle("~/bundles/scripts/default").Include(
                "~/Assets/Scripts/core.js",
                "~/Assets/Scripts/Home/header-nav.js",
                "~/Assets/Scripts/Home/header-msg.js",
                "~/Assets/Scripts/Home/menu-bar.js",
                "~/Assets/Scripts/CustomerProfiles/brief-profile.js",
                "~/Assets/Scripts/Home/index.js",
                "~/Assets/Scripts/Announcement/announcement.js"));
        }

        private static void RegisterHomeStyle(BundleCollection bundles)
        {
            var cssRewriteUrlTransformWrapper = new CssRewriteUrlTransformWrapper();
            bundles.Add(new StyleBundle("~/bundle/styles/default")
                .Include("~/Assets/Styles/Home/page.css", cssRewriteUrlTransformWrapper)
                .Include("~/Assets/Styles/Home/PopupMessage.css", cssRewriteUrlTransformWrapper)
                .Include("~/Assets/Styles/CustomerProfiles/customer-profile.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/Styles/Announcement/announcements.css", cssRewriteUrlTransformWrapper));
        }
    }
}