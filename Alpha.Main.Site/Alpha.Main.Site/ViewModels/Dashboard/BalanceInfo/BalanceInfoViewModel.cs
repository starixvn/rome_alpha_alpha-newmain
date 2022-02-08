namespace Alpha.Main.Site.ViewModels.Dashboard.BalanceInfo
{
    using Alpha.Web.Models;
    using AppResources;
    using Helpers;
    using Main.Models.BalanceInfo;
    using System;

    public class BalanceInfoViewModel
    {
        private const string DefaultCurrency = "UUS";

        private readonly UserLevel userLevel;

        public BalanceInfoViewModel(
            BalanceInfo balanceInfo,
            UserLevel userLevel,
            string currency,
            int custId,
            bool canViewReports,
            Func<int, bool> canViewProduct)
        {
            BalanceInfo = balanceInfo;
            Currency = currency;
            this.userLevel = userLevel;
            CustId = custId;
            SetLabelTextByUserLevel();
            CanViewReports = canViewReports;
            CanViewProduct = canViewProduct;
        }

        public BalanceInfo BalanceInfo { get; }

        public string Currency { get; } = DefaultCurrency;

        public bool CanViewReports { get; }

        public Func<int, bool> CanViewProduct { get; }

        public string UserLevelLabel { get; private set; }

        public string GivenCreditLabel { get; private set; }

        public string TotalDownlineCreditLabel { get; private set; }

        public string Member { get; set; }

        public int CustId { get; }

        public string ReportSiteAppPath => UrlHelper.MapReportsPath("~/");

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
            var dateReport = isToday ? DateTime.Now.ToShortDateString() : DateTime.Now.AddDays(-1).ToShortDateString();
            var userSelectedDateRangeOption = isToday ? 0 : 1;
            var queryString = $"IsFilterVisible=true&SortPropertyName=CustName&IsDescendingSort=False";
            var path = $@"{ReportSiteAppPath}Winloss/Index?{queryString}&UserSelectedDateRangeOption={userSelectedDateRangeOption}&FromDate={dateReport}&ToDate={dateReport}";

            return path;
        }
    }
}
