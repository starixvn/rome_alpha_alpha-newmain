namespace Alpha.Main.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Statistics;
    using Fanex.Data.Repository;
    using Models.Enumerations;
    using Models.Statistics;

    public class StatisticsServiceImpl : StatisticsService
    {
        private readonly BalanceUpDayInfoService balanceUpDayInfoService;
        private readonly IDynamicRepository dynamicRepository;

        public StatisticsServiceImpl(
            BalanceUpDayInfoService balanceUpDayInfoService,
            IDynamicRepository dynamicRepository)
        {
            this.balanceUpDayInfoService = balanceUpDayInfoService;
            this.dynamicRepository = dynamicRepository;
        }

        public Statistics GetStatistics(int custId, UserLevel userLevel)
        {
            var criteria = new GetStatisticsCriteria(userLevel, custId);

            var statistics = dynamicRepository.Get<Statistics>(criteria);

            return statistics;
        }

        public List<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopWinningMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay,
                balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay);

            var topWinninMembers = dynamicRepository.Fetch<TopWinningMember>(criteria).ToList();

            return topWinninMembers;
        }

        public List<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopTurnoverMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay,
                balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay);

            var topTurnoverMembers = dynamicRepository.Fetch<TopTurnoverMember>(criteria).ToList();

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