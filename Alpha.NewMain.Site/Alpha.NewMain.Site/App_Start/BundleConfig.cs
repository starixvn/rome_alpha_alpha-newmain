namespace Alpha.NewMain.Site.App_Start
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
            RegisterPortalPageJavascript(bundles);
            RegisterStatisticsJavascript(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            RegisterMainStyle(bundles);
            RegisterPortalPageStyle(bundles);
            RegisterStatisticsStyle(bundles);
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
                .Include("~/Content/Main/Styles/css/page.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Main/Styles/css/Dashboard.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Main/Styles/css/PopupMessage.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Main/Styles/css/LeftMenu.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/CustomerProfile/Styles/css/CustomerProfile.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Announcement/Styles/css/Announcements.css", new CssRewriteUrlTransformWrapper()));
        }

        private static void RegisterPortalPageStyle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundle/styles/portalpage")
                .Include("~/Content/Report/PortalPage/Styles/css/PortalPage.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Report/PortalPage/Styles/css/BalanceInfo.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Report/PortalPage/Styles/css/Announcement.css", new CssRewriteUrlTransformWrapper()));
        }

        private static void RegisterPortalPageJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/portalpage").Include(
                "~/Views/Shared/Scripts/SharedScripts.js",
                "~/Views/Report/Scripts/PortalPage.js"));
        }

        private static void RegisterStatisticsStyle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundle/styles/statistics")
                .Include("~/Content/Statistics/Styles/css/Statistics.css", new CssRewriteUrlTransformWrapper()));
        }

        private static void RegisterStatisticsJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/statistics").Include(
                "~/Views/Shared/Scripts/SharedScripts.js",
                "~/Views/Statistics/Scripts/Statistics.js"));
        }
    }
}