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

        public Statistics Statistics { get; private set; }

        public UserLevel UserLevel { get; private set; }

        public string OutstandingLabel { get; private set; }

        private string GetOutStandingLabelByLevel(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return Resources.superOutstanding;
            }
            else if (userLevel.IsMasterLevel())
            {
                return Resources.masterOutstanding;
            }
            else
            {
                return Resources.agentOutstanding;
            }
        }
    }
}