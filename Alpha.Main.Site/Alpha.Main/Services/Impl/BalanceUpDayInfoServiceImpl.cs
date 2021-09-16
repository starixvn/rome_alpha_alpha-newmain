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
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public BalanceUpDayInfoServiceImpl(IDynamicRepository dynamicRepository, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public BalanceUpDayInfo GetBalanceUpDayInfo()
        {
            var balanceUpDayInfo = cacheService.Get<BalanceUpDayInfo>(BalanceUpDayKey);

            if (balanceUpDayInfo == null)
            {
                var criteria = new GetBalanceUpDayInfoCriteria();

                balanceUpDayInfo = dynamicRepository.Get<BalanceUpDayInfo>(criteria);

                cacheService.Set(BalanceUpDayKey, balanceUpDayInfo, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
            }

            return balanceUpDayInfo;
        }
    }
}