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
        private BalanceUpDayInfoService balanceUpDayInfoService;
        private IDynamicRepository dynamicRepository;

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
            var criteria = new GetTopWinningMembers(userLevel)
            {
                CustId = custId,
                MoveBetsDay = balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay,
                BalanceUpDay = balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay
            };

            var topWinninMembers = dynamicRepository.Fetch<TopWinningMember>(criteria).ToList();

            return topWinninMembers;
        }

        public List<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopTurnoverMembers(userLevel)
            {
                CustId = custId,
                MoveBetsDay = this.balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay,
                BalanceUpDay = this.balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay
            };

            var topTurnoverMembers = this.dynamicRepository.Fetch<TopTurnoverMember>(criteria).ToList();

            return topTurnoverMembers;
        }

        public List<NewMember> GetNewMembers(int custId)
        {
            var criteria = new GetNewMembersCriteria(custId);            

            var members = this.dynamicRepository.Fetch<NewMember>(criteria).ToList();

            return members;
        }
    }
}