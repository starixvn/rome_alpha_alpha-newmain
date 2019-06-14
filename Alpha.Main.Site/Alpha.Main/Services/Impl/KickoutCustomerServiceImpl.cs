namespace Alpha.Main.Services.Impl
{
    using Criteria;
    using Fanex.Data.Repository;
    using Params;

    public class KickoutCustomerServiceImpl : KickoutCustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public KickoutCustomerServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public void Kickout(KickoutCustomerParam param)
            => dynamicRepository.Execute(new KickoutCustomerCriteria(param));
    }
}
