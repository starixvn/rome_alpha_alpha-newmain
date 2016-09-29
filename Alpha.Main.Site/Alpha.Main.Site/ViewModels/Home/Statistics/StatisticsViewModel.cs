namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using AppResources;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;

    public class StatisticsViewModel
    {
        public StatisticsViewModel(Statistics statistics, UserLevel userLevel)
        {
            this.Statistics = statistics;
            this.UserLevel = userLevel;
        }

        public StatisticsViewModel() : this(null, null)
        {
        }

        public Statistics Statistics { get; private set; }

        public UserLevel UserLevel { get; private set; }

        public string OutstandingLabel
        {
            get
            {
                if (UserLevel.IsSuperLevel())
                {
                    return Resources.superOutstanding;
                }
                else if (UserLevel.IsMasterLevel())
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