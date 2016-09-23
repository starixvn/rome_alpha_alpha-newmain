namespace Alpha.Main.Services.Impl
{
    using Criteria.BalanceInfo;
    using Fanex.Data.Repository;
    using Models.BalanceInfo;
    using Models.Enumerations;

    public class BalanceInfoServiceImpl : BalanceInfoService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";

        private BalanceUpDayInfoService balanceUpDayInfoService;
        private IDynamicRepository dynamicRepository;

        public BalanceInfoServiceImpl(
            BalanceUpDayInfoService balanceUpDayInfoService,
            IDynamicRepository dynamicRepository)
        {
            this.balanceUpDayInfoService = balanceUpDayInfoService;
            this.dynamicRepository = dynamicRepository;
        }

        public BalanceInfo GetBalanceInfo(int custId, UserLevel userLevel)
        {
            var criteria = new GetBalanceInfoCriteria(userLevel)
            {
                CustId = custId,
                MoveBetsDay = this.balanceUpDayInfoService.GetBalanceUpDayInfo().MoveBetsDay
            };

            var balanceInfo = this.dynamicRepository.Get<BalanceInfo>(criteria);

            return balanceInfo;
        }
    }
}