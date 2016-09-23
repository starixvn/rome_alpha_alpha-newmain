namespace Alpha.Main.Site.Requests.BalanceInfo
{
    using Main.Models.Enumerations;
    using MediatR;
    using ViewModels.Home.BalanceInfo;

    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        private int userId;
        private byte userLevelId;
        private string currency;

        public BalanceInfoQuery(int userId, byte userLevelId, string currency)
        {
            this.userId = userId;
            this.userLevelId = userLevelId;
            this.currency = currency;
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

        public string Currency
        {
            get
            {
                return this.currency;
            }
        }
    }
}