namespace Alpha.Main.Site.Requests.BalanceInfo
{
    using Main.Models.Enumerations;
    using MediatR;
    using ViewModels.Home.BalanceInfo;

    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        private int custId;
        private byte userLevelId;
        private string currency;

        public BalanceInfoQuery(int custId, byte userLevelId, string currency)
        {
            this.custId = custId;
            this.userLevelId = userLevelId;
            this.currency = currency;
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

        public string Currency
        {
            get
            {
                return this.currency;
            }
        }
    }
}