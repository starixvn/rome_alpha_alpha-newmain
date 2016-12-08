namespace Alpha.Main.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Statistics;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models.Enumerations;
    using Models.Statistics;

    public class StatisticsServiceImpl : StatisticsService
    {
        private readonly BalanceUpDayInfoService balanceUpDayInfoService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly StatisticsDataAccess statisticsDataAccess;

        public StatisticsServiceImpl(
            BalanceUpDayInfoService balanceUpDayInfoService,
            IDynamicRepository dynamicRepository,
            StatisticsDataAccess statisticsDataAccess)
        {
            this.balanceUpDayInfoService = balanceUpDayInfoService;
            this.dynamicRepository = dynamicRepository;
            this.statisticsDataAccess = statisticsDataAccess;
        }

        public Statistics GetStatistics(int custId, UserLevel userLevel)
        {
            var criteria = new GetStatisticsCriteria(userLevel, custId);

            var statistics = statisticsDataAccess.GetStatistics(criteria, userLevel);

            return statistics;
        }

        public List<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopWinningMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay,
                balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay);

            var topWinninMembers = statisticsDataAccess.GetTopWinningMembers(criteria);

            return topWinninMembers;
        }

        public List<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopTurnoverMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay,
                balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay);

            var topTurnoverMembers = statisticsDataAccess.GetTopTurnoverMembers(criteria);

            return topTurnoverMembers;
        }

        public List<NewMember> GetNewMembers(int custId)
        {
            var criteria = new GetNewMembersCriteria(custId);

            var members = dynamicRepository.Fetch<NewMember>(criteria).ToList();

            return members;
        }
    }
}