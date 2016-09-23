namespace Alpha.Main.Services.Impl
{
    using System;
    using Criteria.BalanceInfo;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.BalanceInfo;

    public class BalanceUpDayInfoServiceImpl : BalanceUpDayInfoService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private IDynamicRepository dynamicRepository;

        public BalanceUpDayInfoServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public BalanceUpDayInfo GetBalanceUpDayInfo()
        {
            var balanceUpDayInfo = CacheService.Get<BalanceUpDayInfo>(BalanceUpDayKey);
            if (balanceUpDayInfo == null)
            {
                var criteria = new GetBalanceUpDayInfoCriteria();

                balanceUpDayInfo = this.dynamicRepository.Get<BalanceUpDayInfo>(criteria);

                CacheService.Set(BalanceUpDayKey, balanceUpDayInfo, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            return balanceUpDayInfo;
        }
    }
}