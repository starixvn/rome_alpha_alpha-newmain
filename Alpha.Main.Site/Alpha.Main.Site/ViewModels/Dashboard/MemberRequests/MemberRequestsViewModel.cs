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

        public int TotalRequest
        {
            get { return ProductRequests.Sum(productRequest => productRequest.NumberOfRequest); }
        }

        public IEnumerable<ProductRequest> ProductRequests { get; }

        public UserLevel UserLevel { get; }

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