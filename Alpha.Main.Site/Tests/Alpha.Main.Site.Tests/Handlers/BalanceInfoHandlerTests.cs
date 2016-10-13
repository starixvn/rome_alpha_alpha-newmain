#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Constants;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Dashboard.BalanceInfo;
    using Site.ViewModels.Dashboard.BalanceInfo;
    using Sunplus.Agent.Model.Entities;

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
            balanceInfoService
                .GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(balanceInfo);

            var query = new BalanceInfoQuery(userSession);

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
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>();
            balanceInfoService
                .GetBalanceInfo(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(balanceInfo);

            var query = new BalanceInfoQuery(userSession);

            var expectedBalanceInfoViewModel = new BalanceInfoViewModel(
                                                        balanceInfo,
                                                        query.UserLevel,
                                                        query.Currency,
                                                        query.CustId);

            // Act
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