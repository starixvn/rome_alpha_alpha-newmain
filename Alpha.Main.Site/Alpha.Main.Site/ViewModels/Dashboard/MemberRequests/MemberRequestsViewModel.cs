namespace Alpha.Main.Site.ViewModels.Dashboard.MemberRequests
{
    using System.Collections.Generic;
    using Main.Helpers;
    using Main.Models.MemberRequests;
    using System.Linq;
    using Main.Models.Enumerations;

    public class MemberRequestsViewModel
    {
        public MemberRequestsViewModel(
            IEnumerable<ProductRequest> productRequests,
            UserLevel userLevel)
        {
            ProductRequests = productRequests;
            UserLevel = userLevel;
        }

        public IEnumerable<ProductRequest> ProductRequests { get; }

        public int TotalRequest => ProductRequests.Sum(productRequest => productRequest.NumberOfRequest);

        public UserLevel UserLevel { get; }

        public string BuildEnableMemberRequest(int productId) 
            => $"{MicroSites.MemberInfoSiteUrl("~/")}/MemberRequests/Index?UserSelectedProductId={productId}";

        public string BuildViewMemberRequestReport() => string.Empty;
    }
}