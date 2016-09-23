namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using Models.BalanceInfo;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Criteria.BalanceInfo;

    public class BalanceUpDayInfoDataAccessImpl : BalanceUpDayInfoDataAccess
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private IDynamicRepository dynamicRepository;

        public BalanceUpDayInfoDataAccessImpl(IDynamicRepository dynamicRepository)
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
