namespace Alpha.Main.Services.Impl
{
    using System.Collections.Generic;
    using Alpha.Web.Models;
    using Criteria.Statistics;
    using Fanex.Data.Repository;
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
            => dynamicRepository.Get<Statistics>(new GetStatisticsCriteria(userLevel, custId));

        public IEnumerable<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopWinningMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay,
                balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay);

            return dynamicRepository.Fetch<TopWinningMember>(criteria);
        }

        public IEnumerable<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel)
        {
            var criteria = new GetTopTurnoverMembersCriteria(
                userLevel,
                custId,
                balanceUpDayInfoService.GetBalanceUpDayInfo().BalanceUpDay,
                balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay);

            return dynamicRepository.Fetch<TopTurnoverMember>(criteria);
        }

        public IEnumerable<NewMember> GetNewMembers(int custId)
            => dynamicRepository.Fetch<NewMember>(new GetNewMembersCriteria(custId));
    }
}