namespace Alpha.Main.Services.Impl
{
    using System;
    using Models.Home;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;
    using Criteria.HomeInfo;
    using Fanex.Caching;

    public class HomeInfoServiceImpl : HomeInfoService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private const string SettingKeyAgent = "Age_CustomerSelBalanceInfo_Ag";
        private const string SettingKeyMaster = "Age_CustomerSelBalanceInfo_MAg";
        private const string SettingKeySuper = "Age_CustomerSelBalanceInfo_SAg";

        private IDynamicRepository dynamicRepository;

        public HomeInfoServiceImpl(IDynamicRepository dynamicRepository)
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

        public BalanceInfo GetBalanceInfo(IUserSession currentUser)
        {
            var criteria = InitBalanceInfoCriteriaForEachUserLevel(currentUser);

            if (criteria == null)
            {
                return null;
            }

            criteria.CustId = currentUser.CustId;
            criteria.MoveBetsDay = GetBalanceUpDayInfo().MoveBetsDay;

            var balanceInfo = this.dynamicRepository.Get<BalanceInfo>(criteria);
            
            return balanceInfo;
        }

        private GetBalanceInfoCriteria InitBalanceInfoCriteriaForEachUserLevel(IUserSession currentUser)
        {
            GetBalanceInfoCriteria criteria = null;

            if (currentUser.IsAgent)
            {
                criteria = new GetBalanceInfoCriteria(SettingKeyAgent);
            }
            else if (currentUser.IsMaster)
            {
                criteria = new GetBalanceInfoCriteria(SettingKeyMaster);
            }
            else if (currentUser.IsSuper)
            {
                criteria = new GetBalanceInfoCriteria(SettingKeySuper);
            }

            return criteria;
        }
    }
}
