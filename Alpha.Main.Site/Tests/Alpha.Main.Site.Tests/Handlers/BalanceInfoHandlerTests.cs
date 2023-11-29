namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using Constants;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Main.Models.BalanceInfo;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Dashboard.BalanceInfo;
    using Site.ViewModels.Dashboard.BalanceInfo;

    [TestFixture]
    public class BalanceInfoHandlerTests
    {
        private BalanceInfoService balanceInfoService;
        private BalanceInfoHandler balanceInfoHandler;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            balanceInfoService = Substitute.For<BalanceInfoService>();
            balanceInfoHandler = new BalanceInfoHandler(balanceInfoService);

            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);
            PageAccess pageAccess = new PageAccess();
            pageAccess.ViewReports = true;
            userSession.SubPageAccess.Returns(pageAccess);
        }

        [Test]
        public void HandleBalanceInfoQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            BalanceInfoQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => balanceInfoHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleBalanceInfoQuery_NotNullQuery_CallGetBalanceInfoService()
        {
            // Arrange
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>();
            balanceInfoService.GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(balanceInfo);

            var query = new BalanceInfoQuery(userSession, productId => true);

            // Act
            balanceInfoHandler.Handle(query);

            // Assert
            balanceInfoService
                .Received()
                .GetBalanceInfo(Arg.Is<int>(cid => cid == query.CustId), Arg.Is<UserLevel>(ul => ul == query.UserLevel));
        }

        [Test]
        public void HandleBalanceInfoQuery_NotNullQuery_ReturnBalanceInfoViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            UrlHelperTestSetUp.SetUp();

            var balanceInfo = FakeDataHelper.Create<BalanceInfo>();
            balanceInfoService.GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(balanceInfo);

            var query = new BalanceInfoQuery(userSession, productId => false);

            var expectedBalanceInfoViewModel = new BalanceInfoViewModel(
                                                        balanceInfo,
                                                        query.UserLevel,
                                                        query.Currency,
                                                        query.CustId,
                                                        true,
                                                        query.CanViewProduct);

            // Act
            var balanceInfoViewModel = balanceInfoHandler.Handle(query);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedBalanceInfoViewModel, balanceInfoViewModel).AreEqual);
        }
    }
}