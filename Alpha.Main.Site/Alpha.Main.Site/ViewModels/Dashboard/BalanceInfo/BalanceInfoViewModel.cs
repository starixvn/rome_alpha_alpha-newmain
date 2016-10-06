namespace Alpha.Main.Site.ViewModels.Dashboard.BalanceInfo
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
            BalanceInfo = balanceInfo;
            Currency = currency;
            SetLabelTextByUserLevel(userLevel);
        }

        public BalanceInfo BalanceInfo { get; }

        public string Currency { get; } = "UUS";

        public string UserLevelLabel { get; private set; }

        public string GivenCreditLabel { get; private set; }

        public string TotalDownlineCreditLabel { get; private set; }

        private void SetLabelTextByUserLevel(UserLevel userLevel)
        {
            if (userLevel.IsAgentLevel())
            {
                UserLevelLabel = Resources.agent;
                GivenCreditLabel = Resources.agentGivenCredit;
                TotalDownlineCreditLabel = Resources.totalMemberCredit;
            }
            else if (userLevel.IsMasterLevel())
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