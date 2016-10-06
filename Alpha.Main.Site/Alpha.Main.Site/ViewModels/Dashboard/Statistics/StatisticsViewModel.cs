namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using AppResources;
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