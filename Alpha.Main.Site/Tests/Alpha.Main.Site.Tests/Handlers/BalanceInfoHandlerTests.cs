namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.BalanceInfo;
    using ViewModels.Home.BalanceInfo;

    [TestFixture]
    public class BalanceInfoHanlderTests
    {
        private BalanceInfoService balanceInfoService;
        private BalanceInfoHandler balanceInfoHandler;

        [SetUp]
        public void SetUp()
        {
            balanceInfoService = Substitute.For<BalanceInfoService>();
            balanceInfoHandler = new BalanceInfoHandler(balanceInfoService);
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
            balanceInfoService.GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(balanceInfo);

            var query = new BalanceInfoQuery(1, 2, "USS");

            // Action
            var viewModel = balanceInfoHandler.Handle(query);

            // Assert
            balanceInfoService.Received().GetBalanceInfo(Arg.Is<int>(cud => cud == query.CustId), Arg.Is<UserLevel>(ul => ul == query.UserLevel));
        }

        [Test]
        public void HandleBalanceInfoQuery_NotNullQuery_ReturnBalanceInfo()
        {
            // Arrange
            var expectedBalanceInfo = new BalanceInfo("user", 1, 1, 1, 1, 1, 1, 1, 1);
            balanceInfoService.GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(expectedBalanceInfo);

            var query = new BalanceInfoQuery(1, 2, "USS");

            // Action
            var viewModel = balanceInfoHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedBalanceInfo, viewModel.BalanceInfo);
        }

        [Test]
        public void HandleBalanceInfoQuery_NotNullQuery_ReturnBalanceInfoViewModel()
        {
            // Arrange
            var balanceInfo = new BalanceInfo("user", 1, 1, 1, 1, 1, 1, 1, 1);
            balanceInfoService.GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(balanceInfo);

            var query = new BalanceInfoQuery(1, 2, "USS");

            var expectedBalanceInfoViewModel =
                new BalanceInfoViewModel(
                    balanceInfo,
                    query.UserLevel,
                    query.Currency);

            // Action
            var balanceInfoViewModel = balanceInfoHandler.Handle(query);

            // Assert
            Assert.IsTrue(expectedBalanceInfoViewModel.Currency == balanceInfoViewModel.Currency
                && expectedBalanceInfoViewModel.GivenCreditLabel == balanceInfoViewModel.GivenCreditLabel
                && expectedBalanceInfoViewModel.TotalDownlineCreditLabel == balanceInfoViewModel.TotalDownlineCreditLabel
                && expectedBalanceInfoViewModel.UserLevelLabel == balanceInfoViewModel.UserLevelLabel);
        }
    }
}