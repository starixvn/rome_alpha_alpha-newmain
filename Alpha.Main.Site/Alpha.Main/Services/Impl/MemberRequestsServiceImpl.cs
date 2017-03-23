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
            ////var productRequests = new List<ProductRequest>
            ////{
            ////    new ProductRequest(5, "Casino", 5),
            ////    new ProductRequest(6, "Fishing World", 15),
            ////    new ProductRequest(7, "Macau Games", 0),
            ////};

            var productRequestData = dynamicRepository.Fetch<ProductRequest>(new ProductRequestCriteria(custId));
            return productRequestData;
        }
    }
}