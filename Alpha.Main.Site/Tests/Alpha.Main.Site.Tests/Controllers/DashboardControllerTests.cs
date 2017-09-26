namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests.Dashboard.BalanceInfo;
    using Alpha.Main.Site.ViewModels.Dashboard;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class DashboardControllerTests
    {
        private DashboardController dashboardController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);
            userSession.SubPageAccess.Returns(new PageAccess());

            dashboardController = new DashboardController(mediator, userSession);
        }

        [Test]
        public void Index_Always_ReturnsViewWithDashboardViewModel()
        {
            // Arrange
            var expectedViewModel = new DashboardViewModel(userSession.RoleId);
            var compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "OldMainRootPath", "MainRootPath" }
            });

            // Act
            var actualView = dashboardController.Index() as ViewResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualView.Model).AreEqual);
        }

        [Test]
        public void BalanceInfo_Always_CallsMediatorSendBalanceInfoQuery()
        {
            // Arrange
            var query = new BalanceInfoQuery(userSession);

            // Act
            dashboardController.BalanceInfo();

            // Assert
            mediator.Received().Send(
                Arg.Is<BalanceInfoQuery>(
                    param => param.CustId == query.CustId
                        && param.UserLevel == query.UserLevel
                        && param.Currency == query.Currency
                        && param.CanViewReports == query.CanViewReports));
        }
    }
}
