namespace Alpha.Main.Site.Requests.Statistics
{
    using Main.Models.Enumerations;
    using MediatR;
    using ViewModels.Home.Statistics;

    public class TopWinningMembersQuery : IRequest<TopWinningMembersViewModel>
    {
        private int custId;
        private byte userLevelId;

        public TopWinningMembersQuery(int custId, byte userLevelId)
        {
            this.custId = custId;
            this.userLevelId = userLevelId;
        }

        public int CustId
        {
            get
            {
                return this.custId;
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