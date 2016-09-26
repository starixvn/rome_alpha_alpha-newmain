namespace Alpha.Main.Site.ViewModels.Home.BalanceInfo
{
    using AppResources;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;

    public class BalanceInfoViewModel
    {
        private BalanceInfo balanceInfo;
        private string currency = "UUS";
        private string userLevelLabel = Resources.agent;
        private string givenCreditLabel = Resources.agentGivenCredit;
        private string totalDownlineCreditLabel = Resources.totalMemberCredit;

        public BalanceInfoViewModel(
            BalanceInfo balanceInfo,
            UserLevel userLevel,
            string currency)
        {
            this.balanceInfo = balanceInfo;
            this.currency = currency;
            SetLabelTextByUserLevel(userLevel);
        }

        public BalanceInfoViewModel() : this(null, null, null)
        {
        }

        public string Currency
        {
            get
            {
                return this.currency;
            }
        }

        public BalanceInfo BalanceInfo
        {
            get
            {
                return this.balanceInfo;
            }
        }

        public string UserLevelLabel { get; private set; }

        public string GivenCreditLabel { get; private set; }

        public string TotalDownlineCreditLabel { get; private set; }

        private void SetLabelTextByUserLevel(UserLevel userLevel)
        {
            if (userLevel.IsMasterLevel())
            {
                UserLevelLabel = Resources.master;
                GivenCreditLabel = Resources.masterGivenCredit;
                TotalDownlineCreditLabel = Resources.totalAgentCredit;
            }
            else if (userLevel.IsSuperLevel())
            {
                UserLevelLabel = Resources.super;
                GivenCreditLabel = Resources.superGivenCredit;
                TotalDownlineCreditLabel = Resources.totalMasterCredit;
            }
        }
    }
}