namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using AppResources;
    using Main.Helpers;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;

    public class StatisticsViewModel
    {
        public StatisticsViewModel(Statistics statistics, UserLevel userLevel)
        {
            Statistics = statistics;
            UserLevel = userLevel;
            OutstandingLabel = GetOutStandingLabelByLevel(userLevel);
        }

        public Statistics Statistics { get; }

        public UserLevel UserLevel { get; }

        public string OutstandingLabel { get; }

        public string MemberOutstandingReportPath
        {
            get
            {
                var reportSiteAppPath = MicroSites.ReportSiteUrl("~/");

                return $@"{reportSiteAppPath}/outstanding/{GetOutstandingDefaultReportLevel(UserLevel)}";
            }
        }

        internal string GetOutstandingDefaultReportLevel(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return "master";
            }

            if (userLevel.IsMasterLevel())
            {
                return "agent";
            }

            return "member";
        }

        private string GetOutStandingLabelByLevel(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return Resources.superOutstanding;
            }

            if (userLevel.IsMasterLevel())
            {
                return Resources.masterOutstanding;
            }

            return Resources.agentOutstanding;
        }
    }
}