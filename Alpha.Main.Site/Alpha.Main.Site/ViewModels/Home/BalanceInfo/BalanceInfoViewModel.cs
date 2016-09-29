namespace Alpha.Main.Site.ViewModels.Home.BalanceInfo
{
    using AppResources;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;

    public class BalanceInfoViewModel
    {
        public BalanceInfoViewModel(
            BalanceInfo balanceInfo,
            UserLevel userLevel,
            string currency)
        {
            this.BalanceInfo = balanceInfo;
            this.Currency = currency;

            SetLabelTextByUserLevel(userLevel);
        }

        public BalanceInfo BalanceInfo { get; private set; }

        public string Currency { get; } = "UUS";

        public string UserLevelLabel { get; private set; } = Resources.agent;

        public string GivenCreditLabel { get; private set; } = Resources.agentGivenCredit;

        public string TotalDownlineCreditLabel { get; private set; } = Resources.totalMemberCredit;

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