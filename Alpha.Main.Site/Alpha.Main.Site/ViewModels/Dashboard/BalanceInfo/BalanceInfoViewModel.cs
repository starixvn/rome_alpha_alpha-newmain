namespace Alpha.Main.Site.ViewModels.Dashboard.BalanceInfo
{
    using System;
    using AppResources;
    using Main.Helpers;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;

    public class BalanceInfoViewModel
    {
        private const string DefaultCurrency = "UUS";

        private readonly UserLevel userLevel;

        public BalanceInfoViewModel(
            BalanceInfo balanceInfo,
            UserLevel userLevel,
            string currency,
            int custId)
        {
            BalanceInfo = balanceInfo;
            Currency = currency;
            this.userLevel = userLevel;
            CustId = custId;
            SetLabelTextByUserLevel();
        }

        public BalanceInfo BalanceInfo { get; }

        public string Currency { get; } = DefaultCurrency;

        public string UserLevelLabel { get; private set; }

        public string GivenCreditLabel { get; private set; }

        public string TotalDownlineCreditLabel { get; private set; }

        public string Member { get; set; }

        public int CustId { get; }

        public string MemberTodayWinLossReportPath => GetMemberWinLossReportPathByDate(true);

        public string MemberYesterdayWinLossReportPath => GetMemberWinLossReportPathByDate(false);

        private void SetLabelTextByUserLevel()
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

        private string GetMemberWinLossReportPathByDate(bool isToday)
        {
            var reportSiteAppPath = MicroSites.ReportSiteUrl("~/");
            var dateReport = isToday ? DateTime.Now.ToShortDateString() : DateTime.Now.AddDays(-1).ToShortDateString();
            var userSelectedDateRangeOption = isToday ? 0 : 1;

            return $@"{reportSiteAppPath}/Winloss/Winloss?IsFilterVisible=true
                   &UserSelectedDateRangeOption={userSelectedDateRangeOption}&FromDate={dateReport}&ToDate={dateReport}";
        }
    }
}