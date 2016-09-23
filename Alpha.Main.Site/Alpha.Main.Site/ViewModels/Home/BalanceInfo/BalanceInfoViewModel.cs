namespace Alpha.Main.Site.ViewModels.Home.BalanceInfo
{
    using AppResources;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;

    public class BalanceInfoViewModel
    {
        private BalanceInfo balanceInfo;
        private string currency = string.Empty;
        private string agentLabel = string.Empty;
        private string agentGivenCreditLabel = string.Empty;
        private string downlineCreditLabel = string.Empty;

        public BalanceInfoViewModel(
            BalanceInfo balanceInfo,
            UserLevel userLevel,
            string currency)
        {
            this.balanceInfo = balanceInfo;
            this.currency = currency;
            InitLabelNameByUserLevel(userLevel);
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

        public string AgentLabel { get; private set; }

        public string AgentGivenCreditLabel { get; private set; }

        public string DownlineCreditLabel { get; private set; }

        private void InitLabelNameByUserLevel(UserLevel userLevel)
        {
            if (userLevel.IsAgentLevel())
            {
                AgentLabel = Resources.agent;
                AgentGivenCreditLabel = Resources.agentGivenCredit;
                DownlineCreditLabel = Resources.totalMemberCredit;
            }
            else if (userLevel.IsMasterLevel())
            {
                AgentLabel = Resources.master;
                AgentGivenCreditLabel = Resources.masterGivenCredit;
                DownlineCreditLabel = Resources.totalAgentCredit;
            }
            else if (userLevel.IsSuperLevel())
            {
                AgentLabel = Resources.super;
                AgentGivenCreditLabel = Resources.superGivenCredit;
                DownlineCreditLabel = Resources.totalMasterCredit;
            }
        }
    }
}