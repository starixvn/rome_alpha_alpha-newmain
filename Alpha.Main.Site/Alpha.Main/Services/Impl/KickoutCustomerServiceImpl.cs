namespace Alpha.Main.Services.Impl
{
    using System;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using Params;

    public class KickoutCustomerServiceImpl : KickoutCustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public KickoutCustomerServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public void Kickout(KickoutCustomerParam param)
        {
            try
            {
                if (!param.IsDisabledRecheckin)
                {
                    dynamicRepository.Execute(new KickoutCustomerCriteria(param));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
