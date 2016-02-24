namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.Linq;
    using Alpha.NewMain.Site.Extensions;
    using Criteria.Report;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Dtos;
    using Models.Views.Report;
    using Sunplus.Agent.Model.Entities;

    public class ReportService : IReportService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";

        private readonly IDynamicRepository _dynamicRepository;

        public ReportService(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public DateTime BalanceUpDate
        {
            get
            {
                return Convert.ToDateTime(GetBalanceUpDay().BalanceUpDay);
            }
        }

        public DateTime MoveBetsDate
        {
            get
            {
                return Convert.ToDateTime(GetBalanceUpDay().MoveBetsDay);
            }
        }

        public BalanceInfoViewModel GetBalanceInfo()
        {
            var result = new BalanceInfoViewModel
            {
                Today = DateTime.Now.ToShortFormatedDateWithSlash(),
                Yesterday = DateTime.Now.AddDays(-1).ToShortFormatedDateWithSlash()
            };

            if (UserSession.Current.IsSuper)
            {
                result.RoleName = App_GlobalResources.Resources.super;
                result.GivenCreditText = App_GlobalResources.Resources.superGivenCredit;
                result.TotalDownlineCreditText = App_GlobalResources.Resources.totalMasterCredit;
            }
            else if (UserSession.Current.IsMaster)
            {
                result.RoleName = App_GlobalResources.Resources.master;
                result.GivenCreditText = App_GlobalResources.Resources.masterGivenCredit;
                result.TotalDownlineCreditText = App_GlobalResources.Resources.totalAgentCredit;
            }
            else if (UserSession.Current.IsAgent)
            {
                result.RoleName = App_GlobalResources.Resources.agent;
                result.GivenCreditText = App_GlobalResources.Resources.agentGivenCredit;
                result.TotalDownlineCreditText = App_GlobalResources.Resources.totalMemberCredit;
            }

            var date = MoveBetsDate;
            var rawData = _dynamicRepository.Fetch<BalanceInfoModel>(new GetBalanceInfoCriteria
            {
                CustId = UserSession.Current.CustId,
                MoveBetsDay = MoveBetsDate
            }).FirstOrDefault();

            if (UserSession.Current.Language == "ko-KR" &&
                UserSession.Current.Currency == "Won")
            {
                result.Currency = "원 ";
            }
            else
            {
                result.Currency = UserSession.Current.Currency + " ";
            }

            if (rawData != null)
            {
                result.HasData = true;
                result.CashBalance = FormatDecimal_BalanceInfo(rawData.CashBalance);
                result.YesterdayCashBalance = FormatDecimal_BalanceInfo(rawData.YesterdayCashBalance);
                result.TotalBalance = FormatDecimal_BalanceInfo(rawData.TotalBalance);
                result.YesterdayTotalBalance = FormatDecimal_BalanceInfo(rawData.YesterdayTotalBalance);
                result.TodayWinLoss = FormatDecimal_BalanceInfo(rawData.TodayWinLoss);
                result.YesterdayWinLoss = FormatDecimal_BalanceInfo(rawData.YesterdayWinLoss);
                result.GivenCredit = FormatDecimal_BalanceInfo(rawData.GivenCredit);
                result.TotalDownlineCredit = FormatDecimal_BalanceInfo(rawData.MemberCredit);
            }

            return result;
        }

        public BalanceUpDayModel GetBalanceUpDay()
        {
            var cache = CacheService.CreateInstance(CacheType.MemoryCache);

            var oldData = cache.Get<BalanceUpDayModel>(BalanceUpDayKey);
            if (oldData == null)
            {
                var rawData = _dynamicRepository.Fetch<BalanceUpDayModel>(new GetBalanceUpDayCriteria());
                var result = rawData.FirstOrDefault();
                cache.Set<BalanceUpDayModel>(BalanceUpDayKey, result, DateTime.Now.AddMinutes(5));
                return result;
            }

            return oldData as BalanceUpDayModel;
        }

        private string FormatDecimal_BalanceInfo(object value)
        {
            var format = "<span class='{0}'>{1}</span>";
            var num = decimal.Round(Convert.ToDecimal(value == DBNull.Value ? 0 : value), 2);
            var style = num < .00m ? "note-text" : "underdog-text";
            var ret = string.Format(format, style, num.Format(2));
            return ret;
        }
    }
}