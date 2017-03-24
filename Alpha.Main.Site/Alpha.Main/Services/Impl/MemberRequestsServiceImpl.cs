namespace Alpha.Main.Services.Impl
{
    using System.Collections.Generic;
    using Criteria.MemberRequests;
    using Fanex.Data.Repository;
    using Models.MemberRequests;

    public class MemberRequestsServiceImpl : MemberRequestsService
    {
        private readonly IDynamicRepository dynamicRepository;

        public MemberRequestsServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<ProductRequest> GetProductRequestData(int custId)
        {
            var productRequestData = dynamicRepository.Fetch<ProductRequest>(new ProductRequestCriteria(custId));
            return productRequestData;
        }
    }
}