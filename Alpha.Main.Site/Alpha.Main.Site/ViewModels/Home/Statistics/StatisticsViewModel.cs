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
        }

        public StatisticsViewModel() : this(null, null)
        {
        }

        public Statistics Statistics
        {
            get
            {
                return this.statistics;
            }
        }

        public UserLevel UserLevel
        {
            get
            {
                return this.userLevel;
            }
        }

        public string OutstandingLabel
        {
            get
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
}