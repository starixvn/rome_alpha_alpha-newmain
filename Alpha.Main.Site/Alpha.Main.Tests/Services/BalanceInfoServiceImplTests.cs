namespace Alpha.Main.Tests.Services
{
    using System;
    using Fanex.Data.Repository;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceInfoServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private BalanceUpDayInfoService balanceUpDateInfoService;
        private BalanceInfoServiceImpl balanceInfoService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            balanceUpDateInfoService = Substitute.For<BalanceUpDayInfoService>();
            balanceUpDateInfoService.GetBalanceUpDayInfo().Returns(new BalanceUpDayInfo(DateTime.Today, DateTime.Today));

            balanceInfoService = new BalanceInfoServiceImpl(balanceUpDateInfoService, dynamicRepository);
        }

        [Test]
        public void GetBalanceInfo_InvalidCustId_ReturnNull()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(2);

            // Action
            var balanceInfo = balanceInfoService.GetBalanceInfo(1, userLevel);

            // Assert
            Assert.AreEqual(balanceInfo, null);
        }

        [Test]
        public void GetBalanceInfo_ValidCustId_ReturnBalanceInfo()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(2);
            var expectedBalanceInfo = new BalanceInfo("user", 1, 1, 1, 1, 1, 1, 1, 1);

            dynamicRepository.Get<BalanceInfo>(Arg.Any<object>()).Returns(expectedBalanceInfo);

            // Action
            var balanceInfo = balanceInfoService.GetBalanceInfo(1, userLevel);

            // Assert
            Assert.AreEqual(balanceInfo, expectedBalanceInfo);
        }
    }
}