namespace Alpha.Main.Services.Impl
{
    using Models.Statistics;
    using Fanex.Data.Repository;
    using Criteria.Statistics;
    using System.Collections.Generic;
    using Models.Enumerations;
    using DataAccesses;

    public class StatisticsServiceImpl : StatisticsService
    {
        private BalanceUpDayInfoDataAccess balanceUpDayInfoDA;
        private IDynamicRepository dynamicRepository;

        public StatisticsServiceImpl(
            BalanceUpDayInfoDataAccess balanceUpDayInfoDA
            , IDynamicRepository dynamicRepository)
        {
            this.balanceUpDayInfoDA = balanceUpDayInfoDA;
            this.dynamicRepository = dynamicRepository;
        }

        public Statistics GetStatistics(int custId, UserLevel userLevel)
        {
            var criteria = new GetStatisticsCriteria(userLevel)
            {
                CustId = custId
            };

            var statistics = this.dynamicRepository.Get<Statistics>(criteria);

            return statistics;
        }

        public IEnumerable<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopWinningMembers(userLevel)
            {
                CustId = custId,
                MoveBetsDay = this.balanceUpDayInfoDA.GetBalanceUpDayInfo().MoveBetsDay,
                BalanceUpDay = this.balanceUpDayInfoDA.GetBalanceUpDayInfo().BalanceUpDay
            };

            var topWinninMembers = this.dynamicRepository.Fetch<TopWinningMember>(criteria);

            return topWinninMembers;
        }

        public IEnumerable<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopTurnoverMembers(userLevel)
            {
                CustId = custId,
                MoveBetsDay = this.balanceUpDayInfoDA.GetBalanceUpDayInfo().MoveBetsDay,
                BalanceUpDay = this.balanceUpDayInfoDA.GetBalanceUpDayInfo().BalanceUpDay
            };

            var topTurnoverMembers = this.dynamicRepository.Fetch<TopTurnoverMember>(criteria);

            return topTurnoverMembers;
        }

        public IEnumerable<NewMember> GetNewMembers(int custId)
        {
            var criteria = new GetNewMembersCriteria()
            {
                CustId = custId
            };

            var newMembers = this.dynamicRepository.Fetch<NewMember>(criteria);

            return newMembers;
        }
    }
}
