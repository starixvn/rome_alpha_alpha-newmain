namespace Alpha.NewMain.Site.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Report.Statistics;
    using Fanex.Data.Repository;
    using Models.Dtos;
    using Models.Dtos.Statistics;
    using Sunplus.Agent.Model.Entities;

    public class StatisticsService : IStatisticsService
    {
        private readonly IDynamicRepository _dynamicRepository;

        private readonly IReportService _reportService;

        public StatisticsService(
            IDynamicRepository dynamicRepository,
            IReportService reportService)
        {
            _dynamicRepository = dynamicRepository;
            _reportService = reportService;
        }

        public StatisticsModel GetStatistics()
        {
            var rawData = _dynamicRepository.Fetch<StatisticsModel>(new StatisticsCriteria
            {
                CustId = UserSession.Current.CustId
            }).FirstOrDefault();

            return rawData;
        }

        public IEnumerable<CustomerInfoModel> Top10WinningOfMonth()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var rawData = _dynamicRepository.Fetch<CustomerInfoModel>(new Top10WinningOfMonthCriteria
            {
                CustId = UserSession.Current.CustId,
                BalanceUpDay = balanceUpDay.BalanceUpDay,
                MoveBetsDay = balanceUpDay.MoveBetsDay
            });

            return rawData;

        }

        public IEnumerable<CustomerInfoModel> HighestTurnoverOfMonth()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var rawData = _dynamicRepository.Fetch<CustomerInfoModel>(new HighestTurnoverOfMonthCriteria
            {
                CustId = UserSession.Current.CustId,
                BalanceUpDay = balanceUpDay.BalanceUpDay,
                MoveBetsDay = balanceUpDay.MoveBetsDay
            });

            return rawData;
        }

        public IEnumerable<CustomerInfoModel> NewMembersOfMonth()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var rawData = _dynamicRepository.Fetch<CustomerInfoModel>(new NewMembersOfMonthCriteria
            {
                CustId = UserSession.Current.CustId
            });

            return rawData;
        }
    }
}