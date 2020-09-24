namespace Alpha.Main.Site.ViewModels.Dashboard.MemberRequests
{
    using Alpha.Web.Models;
    using Helpers;
    using Main.Models.MemberRequests;
    using System.Collections.Generic;
    using System.Linq;

    public class MemberRequestsViewModel
    {
        public MemberRequestsViewModel(
            IEnumerable<ProductRequest> productRequests,
            UserLevel userLevel,
            bool canViewReports,
            bool canViewMemberInfo)
        {
            ProductRequests = productRequests;
            UserLevel = userLevel;
            CanViewReports = canViewReports;
            CanViewMemberInfo = canViewMemberInfo;
        }

        public IEnumerable<ProductRequest> ProductRequests { get; }

        public int TotalRequest => ProductRequests.Sum(productRequest => productRequest.NumberOfRequest);

        public bool CanViewReports { get; }

        public bool CanViewMemberInfo { get; }

        public UserLevel UserLevel { get; }

        public string BuildEnableMemberRequest(int productId)
            => $"{UrlHelper.MapMemberInfoPath("~/")}/MemberRequests/Index?UserSelectedProductId={productId}";

        public string BuildViewMemberRequestReport(int productId)
        {
            var action = UserLevel.IsAgentLevel() ? "Member" : "MasterOrAgent";

            return $"{UrlHelper.MapReportsPath("~/")}/PlayForFun/{action}?UserSelectedProductId={productId}";
        }
    }
}
