namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using AppResources;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;

    public class StatisticsViewModel
    {
        private Statistics statistics;
        private UserLevel userLevel;

        public StatisticsViewModel(Statistics statistics, UserLevel userLevel)
        {
            this.statistics = statistics;
            this.userLevel = userLevel;
            InitForEachUserLevel(userLevel);
        }

        public StatisticsViewModel() : this(null, null)
        {
        }

        public Statistics Statistics { get { return this.statistics; } }

        public UserLevel UserLevel { get { return this.userLevel; } }

        public string AgentOutstandingLabel { get; private set; }

        private void InitForEachUserLevel(UserLevel userLevel)
        {
            if (userLevel.IsAgentLevel())
            {
                AgentOutstandingLabel = Resources.agentOutstanding;
            }
            else if (userLevel.IsMasterLevel())
            {
                AgentOutstandingLabel = Resources.masterOutstanding;
            }
            else if (userLevel.IsSuperLevel())
            {
                AgentOutstandingLabel = Resources.superOutstanding;
            }
        }
    }
}