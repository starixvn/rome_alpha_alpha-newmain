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
            => $"{MicroSites.MemberInfoSitePath("~/")}/MemberRequests/Index?UserSelectedProductId={productId}";

        public string BuildViewMemberRequestReport(int productId)
        {
            var action = UserLevel.IsAgentLevel() ? "Member" : "MasterOrAgent";

            return $"{MicroSites.ReportSitePath("~/")}/PlayForFun/{action}?UserSelectedProductId={productId}";
        }
    }
}