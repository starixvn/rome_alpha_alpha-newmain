namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.Linq;
    using Criteria.Report;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Dtos;
    using Sunplus.Agent.Model.Entities;

    public class ReportService : IReportService
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";

        private readonly IDynamicRepository _dynamicRepository;

        public ReportService(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public DateTime BalanceUpDate
        {
            get
            {
                return GetBalanceUpDay().BalanceUpDay;
            }
        }

        public DateTime MoveBetsDate
        {
            get
            {
                return GetBalanceUpDay().MoveBetsDay;
            }
        }

        public BalanceInfoModel GetBalanceInfo()
        {
            var rawData = _dynamicRepository.Fetch<BalanceInfoModel>(new BalanceInfoCriteria
            {
                CustId = UserSession.Current.CustId,
                MoveBetsDay = MoveBetsDate
            }).FirstOrDefault();

            return rawData;
        }

        public BalanceUpDayModel GetBalanceUpDay()
        {
            var cache = CacheService.CreateInstance(CacheType.MemoryCache);

            var oldData = cache.Get<BalanceUpDayModel>(BalanceUpDayKey);
            if (oldData == null)
            {
                var rawData = _dynamicRepository.Fetch<BalanceUpDayModel>(new BalanceUpDayCriteria()).FirstOrDefault();
                cache.Set<BalanceUpDayModel>(BalanceUpDayKey, rawData, DateTime.Now.AddMinutes(5));
                return rawData;
            }

            return oldData as BalanceUpDayModel;
        }
    }
}