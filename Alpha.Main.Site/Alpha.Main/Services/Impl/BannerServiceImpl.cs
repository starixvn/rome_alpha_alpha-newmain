namespace Alpha.Main.Services.Impl
{
    using System.Threading.Tasks;
    using Alpha.Main.Configurations;
    using Alpha.Main.Criteria.Banner;
    using Alpha.Main.Models.Banner;
    using Fanex.Data.Repository;

    public class BannerServiceImpl : BannerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BannerServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Task<BannerItem> GetBanner(string languageCode, byte type)
            => dynamicRepository.GetAsync<BannerItem>(
                new GetBannerCriteria(languageCode, AppSettings.BannerEnvironment, AppSettings.SiteLabel, type));
    }
}
