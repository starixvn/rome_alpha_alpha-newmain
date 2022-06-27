namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Configurations;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests.Dashboard.BalanceInfo;
    using Alpha.Main.Site.ViewModels.Dashboard;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;
    using TelegramBot.Client.Services;

    [TestFixture]
    public class DashboardControllerTests
    {
        private DashboardController dashboardController;
        private IMediator mediator;
        private IUserSession userSession;
        private IBotService botService;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);
            userSession.SubPageAccess.Returns(new PageAccess());
            botService = Substitute.For<IBotService>();

            dashboardController = new DashboardController(mediator, userSession, botService);
        }

        [Test]
        public async Task Index_Always_ReturnsViewWithDashboardViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            UrlHelperTestSetUp.SetUp();
            AppSettings.Settings["IsShowEmptyDashboard"] = "0";

            var expectedViewModel = new DashboardViewModel(userSession.RoleId);

            // Act
            var actualView = (await dashboardController.Index()) as ViewResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualView.Model).AreEqual);
        }

        [Test]
        public void BalanceInfo_Always_CallsMediatorSendBalanceInfoQuery()
        {
            // Arrange
            var query = new BalanceInfoQuery(userSession, productId => true);

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
