namespace Alpha.Main.Site.Requests.Statistics
{
    using Main.Models.Enumerations;
    using MediatR;
    using ViewModels.Home.Statistics;

    public class StatisticsQuery : IRequest<StatisticsViewModel>
    {
        private int userId;
        private byte userLevelId;

        public StatisticsQuery(int userId, byte userLevelId)
        {
            this.userId = userId;
            this.userLevelId = userLevelId;
        }

        public int UserId
        {
            get
            {
                return this.userId;
            }
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.userLevelId);
            }
        }
    }
}