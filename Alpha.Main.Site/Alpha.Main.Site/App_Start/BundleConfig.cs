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
                "~/Content/Home/Scripts/header.nav.js",
                "~/Content/Home/Scripts/header.msg.js",
                "~/Content/Home/Scripts/menubar.js",
                "~/Content/CustomerProfile/Scripts/brief-profile.js",
                "~/Content/Home/Scripts/IndexPage.js",
                "~/Content/Announcement/Scripts/AnnouncementManager.js"));
        }

        private static void RegisterHomeStyle(BundleCollection bundles)
        {
            var cssRewriteUrlTransformWrapper = new CssRewriteUrlTransformWrapper();
            bundles.Add(new StyleBundle("~/bundle/styles/home")
                .Include("~/Content/Home/Styles/page.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/Home/Styles/PopupMessage.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/CustomerProfile/Styles/CustomerProfile.css", cssRewriteUrlTransformWrapper)
                .Include("~/Content/Announcement/Styles/Announcements.css", cssRewriteUrlTransformWrapper));
        }
    }
}