namespace Alpha.Main.Site.ViewModels.Dashboard.MemberRequests
{
    using System.Collections.Generic;
    using Main.Helpers;
    using Main.Models.MemberRequests;
    using System.Linq;

    public class MemberRequestsViewModel
    {
        public MemberRequestsViewModel(IEnumerable<ProductRequest> productRequests)
        {
            ProductRequests = productRequests;
        }

        public int TotalRequest
        {
            get { return ProductRequests.Sum(productRequest => productRequest.NumberOfRequest); }
        }

        public IEnumerable<ProductRequest> ProductRequests { get; }

        public string BuildEnableMemberRequest(int productId)
        {
            return $"{MicroSites.MemberInfoSiteUrl("~/")}/MemberRequests/Index?UserSelectedProductId={productId}";
        }

        public string BuildViewMemberRequestReport()
        {
            return string.Empty;
        }
    }
}