namespace Alpha.Main.Services.Impl
{
    using Alpha.Web.Models;
    using Criteria.BalanceInfo;
    using Fanex.Data.Repository;
    using Models.BalanceInfo;

    public class BalanceInfoServiceImpl : BalanceInfoService
    {
        private readonly BalanceUpDayInfoService balanceUpDayInfoService;
        private readonly IDynamicRepository dynamicRepository;

        public BalanceInfoServiceImpl(
            BalanceUpDayInfoService balanceUpDayInfoService,
            IDynamicRepository dynamicRepository)
        {
            this.balanceUpDayInfoService = balanceUpDayInfoService;
            this.dynamicRepository = dynamicRepository;
        }

        public BalanceInfo GetBalanceInfo(int custId, UserLevel userLevel)
        {
            var moveBetsDay = balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay;
            var criteria = new GetBalanceInfoCriteria(userLevel, custId, moveBetsDay);

            var balanceInfo = dynamicRepository.Get<BalanceInfo>(criteria);

            return balanceInfo;
        }
    }
}