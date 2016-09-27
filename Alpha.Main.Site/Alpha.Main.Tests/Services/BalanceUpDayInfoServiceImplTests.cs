namespace Alpha.Main.Tests.Services
{
    using System;
    using Fanex.Data.Repository;
    using Main.Models.BalanceInfo;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpDayInfoServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private BalanceUpDayInfoServiceImpl balanceUpDayInfoService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            balanceUpDayInfoService = new BalanceUpDayInfoServiceImpl(dynamicRepository);
        }

        [Test]
        public void GetBalanceUpDayInfo_AnyCustIdAndUserLevel_ReturnBalanceUpDayInfo()
        {
            // Arrange
            var expectedBalanceUpDayInfo = new BalanceUpDayInfo(DateTime.Today, DateTime.Today);
            dynamicRepository.Get<BalanceUpDayInfo>(Arg.Any<object>()).Returns(expectedBalanceUpDayInfo);

            // Action
            var balanceUpDayInfo = balanceUpDayInfoService.GetBalanceUpDayInfo();

            // Assert
            Assert.AreEqual(balanceUpDayInfo, expectedBalanceUpDayInfo);
        }
    }
}