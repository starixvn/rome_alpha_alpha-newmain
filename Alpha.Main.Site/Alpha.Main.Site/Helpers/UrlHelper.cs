namespace Alpha.Main.Site.Helpers
{
    using System;
    using System.Web;
    using Alpha.Main.Configurations;
    using Web.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapExTotalBetsForecastPath;

        [ThreadStatic]
        private static Func<string, string> mapMessagesPath;

        [ThreadStatic]
        private static Func<string, string> mapOldMainPath;

        [ThreadStatic]
        private static Func<string, string> mapHowToUsePath;

        //[ThreadStatic]
        private static Func<string, string> mapReportsPath;

        [ThreadStatic]
        private static Func<string, string> mapMemberInfoPath;

        [ThreadStatic]
        private static Func<string, string> mapAccountInfoPath;

        [ThreadStatic]
        private static Func<string, string> mapMainPath;

        public static Func<string, string> MapExTotalBetsForecastPath
        {
            get
            {
                return mapExTotalBetsForecastPath ?? MicroSites.ExTotalBetsForecastUrl;
            }

            internal set
            {
                mapExTotalBetsForecastPath = value;
            }
        }

        public static Func<string, string> MapMessagesPath
        {
            get
            {
                return mapMessagesPath ?? MicroSites.MessageUrl;
            }

            internal set
            {
                mapMessagesPath = value;
            }
        }

        public static Func<string, string> MapOldMainPath
        {
            get
            {
                return mapOldMainPath ?? MicroSites.OldMainUrl;
            }

            internal set
            {
                mapOldMainPath = value;
            }
        }

        public static Func<string, string> MapHowToUsePath
        {
            get
            {
                return mapHowToUsePath ?? MicroSites.HowToUseUrl;
            }

            internal set
            {
                mapHowToUsePath = value;
            }
        }

        public static Func<string, string> MapReportsPath
        {
            get
            {
                return mapReportsPath ?? MicroSites.ReportsUrl;
            }

            internal set
            {
                mapReportsPath = value;
            }
        }

        public static Func<string, string> MapMemberInfoPath
        {
            get
            {
                return mapMemberInfoPath ?? MicroSites.MemberInfoUrl;
            }

            internal set
            {
                mapMemberInfoPath = value;
            }
        }

        public static Func<string, string> MapAccountInfoPath
        {
            get
            {
                return mapAccountInfoPath ?? MicroSites.AccountInfoUrl;
            }

            internal set
            {
                mapAccountInfoPath = value;
            }
        }

        public static Func<string, string> MapMainPath
        {
            get
            {
                return mapMainPath ?? HttpContext.Current.Response.ApplyAppPathModifier;
            }

            internal set
            {
                mapMainPath = value;
            }
        }

        internal static string MapReportsDomain(string virtualPath = "~/")
            => $"{AppSettings.ReportDomain}/(S({HttpContext.Current.Session.SessionID}))/";
    }
}
