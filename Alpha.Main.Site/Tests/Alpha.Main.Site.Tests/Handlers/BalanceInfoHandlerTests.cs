namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Constants;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.BalanceInfo;
    using Site.ViewModels.Dashboard;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class BalanceInfoHanlderTests
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
        }

        [Test]
        public void HandleBalanceInfoQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            BalanceInfoQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => balanceInfoHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleBalanceInfoQuery_NotNullQuery_CallGetBalanceInfoService()
        {
            // Arrange
            var balanceInfo = new BalanceInfo("user", 1, 1, 1, 1, 1, 1, 1, 1);
            balanceInfoService
                .GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(balanceInfo);

            var query = new BalanceInfoQuery(userSession);

            // Action
            var viewModel = balanceInfoHandler.Handle(query);

            // Assert
            balanceInfoService
                .Received()
                .GetBalanceInfo(Arg.Is<int>(cid => cid == query.CustId), Arg.Is<UserLevel>(ul => ul == query.UserLevel));
        }

        [Test]
        public void HandleBalanceInfoQuery_NotNullQuery_ReturnBalanceInfoViewModel()
        {
            // Arrange
            var balanceInfo = new BalanceInfo("user", 1, 1, 1, 1, 1, 1, 1, 1);
            balanceInfoService
                .GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(balanceInfo);

            var query = new BalanceInfoQuery(userSession);

            var expectedBalanceInfoViewModel = new BalanceInfoViewModel(
                                                        balanceInfo,
                                                        query.UserLevel,
                                                        query.Currency);

            // Action
            var balanceInfoViewModel = balanceInfoHandler.Handle(query);

            // Assert
            Assert.IsTrue(expectedBalanceInfoViewModel.BalanceInfo == balanceInfo
                && expectedBalanceInfoViewModel.Currency == balanceInfoViewModel.Currency
                && expectedBalanceInfoViewModel.GivenCreditLabel == balanceInfoViewModel.GivenCreditLabel
                && expectedBalanceInfoViewModel.TotalDownlineCreditLabel == balanceInfoViewModel.TotalDownlineCreditLabel
                && expectedBalanceInfoViewModel.UserLevelLabel == balanceInfoViewModel.UserLevelLabel);
        }
    }
}