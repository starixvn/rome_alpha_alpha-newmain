namespace Alpha.Main.Services
{
    using System.Collections.Generic;
    using Models.MemberRequests;

    public interface MemberRequestsService
    {
        IEnumerable<ProductRequest> GetProductRequestData(int custId);
    }
}