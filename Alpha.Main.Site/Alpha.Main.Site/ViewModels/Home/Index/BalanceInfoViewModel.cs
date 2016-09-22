namespace Alpha.Main.Site.ViewModels.Home.Index
{
    using AppResources;
    using Main.Models.Home;
    using Sunplus.Agent.Model.Entities;

    public class BalanceInfoViewModel
    {
        private IUserSession currentUser;
        private BalanceInfo balanceInfo;
        private string agentLabel = string.Empty;
        private string agentGivenCreditLabel = string.Empty;
        private string totalMemberCreditLabel = string.Empty;

        public BalanceInfoViewModel(IUserSession currentUser, BalanceInfo balanceInfo)
        {
            this.balanceInfo = balanceInfo;
            this.currentUser = currentUser;
            InitLabel();
        }

        public string AgentName { get { return this.currentUser.UserName; } }

        public string Currency { get { return this.currentUser.Currency; } }

        public BalanceInfo BalanceInfo { get { return this.balanceInfo; } }

        public string AgentLabel { get { return this.agentLabel; } }

        public string AgentGivenCreditLabel { get { return this.agentGivenCreditLabel; } }

        public string TotalMemberCreditLabel { get { return this.totalMemberCreditLabel; } }

        private void InitLabel()
        {
            if (this.currentUser.IsAgent)
            {
                this.agentLabel = Resources.agent;
                this.agentGivenCreditLabel = Resources.agentGivenCredit;
                this.totalMemberCreditLabel = Resources.totalMemberCredit;
            }
            else if (this.currentUser.IsMaster)
            {
                this.agentLabel = Resources.master;
                this.agentGivenCreditLabel = Resources.masterGivenCredit;
                this.totalMemberCreditLabel = Resources.totalAgentCredit;
            }
            else if (this.currentUser.IsSuper)
            {
                this.agentLabel = Resources.super;
                this.agentGivenCreditLabel = Resources.superGivenCredit;
                this.totalMemberCreditLabel = Resources.totalMasterCredit;
            }
        }
    }
}