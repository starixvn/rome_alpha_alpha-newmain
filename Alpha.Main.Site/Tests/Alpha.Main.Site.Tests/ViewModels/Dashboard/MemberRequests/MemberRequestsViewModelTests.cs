namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.MemberRequests
{
    using System.Linq;
    using Helpers;
    using Main.Constants;
    using Main.Models.Enumerations;
    using Main.Models.MemberRequests;
    using NUnit.Framework;
    using Site.ViewModels.Dashboard.MemberRequests;

    [TestFixture]
    public class MemberRequestsViewModelTests
    {
        [Test]
        public void BuildEnableMemberRequest_Always_ReturnsCorrectLink()
        {
            // Arrange
            UrlHelperTestSetUp.SetUp();

            var productId = 1;
            var viewModel = new MemberRequestsViewModel(Enumerable.Empty<ProductRequest>(), UserLevel.Agent, false, false);

            // Act
            var actualLink = viewModel.BuildEnableMemberRequest(productId);

            // Assert
            Assert.AreEqual("MemberInfoPath/MemberRequests/Index?UserSelectedProductId=1", actualLink);
        }

        [TestCase(RoleId.Agent, "Member")]
        [TestCase(RoleId.Master, "MasterOrAgent")]
        [TestCase(RoleId.Super, "MasterOrAgent")]
        public void BuildViewMemberRequestReport_Always_ReturnsCorrectLink(byte roleId, string action)
        {
            // Arrange
            UrlHelperTestSetUp.SetUp();

            var productId = 1;
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);
            var viewModel = new MemberRequestsViewModel(Enumerable.Empty<ProductRequest>(), userLevel, false, false);

            var expectedLink = $"ReportsPath/PlayForFun/{action}?UserSelectedProductId=1";

            // Act
            var actualLink = viewModel.BuildViewMemberRequestReport(productId);

            // Assert
            Assert.AreEqual(expectedLink, actualLink);
        }
    }
}
