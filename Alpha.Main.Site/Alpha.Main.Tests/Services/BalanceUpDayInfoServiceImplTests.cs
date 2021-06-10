namespace Alpha.Main.Tests.Services
{
    using System;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Main.Models.BalanceInfo;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpDayInfoServiceImplTests
    {
        private const string BalanceUpDayKey = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";
        private IDynamicRepository dynamicRepository;
        private ICacheService cacheService;
        private BalanceUpDayInfoServiceImpl balanceUpDayInfoService;
        private ICacheService CacheService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheService = Substitute.For<ICacheService>();

            balanceUpDayInfoService = new BalanceUpDayInfoServiceImpl(dynamicRepository, cacheService);
        }

        [Test]
        public void GetBalanceUpDayInfo_AnyCustIdAndUserLevel_ReturnBalanceUpDayInfo()
        {
            // Arrange
            BalanceUpDayInfo cachedBalanceUpDayInfo = null;
            cacheService.Get<BalanceUpDayInfo>(BalanceUpDayKey).Returns(cachedBalanceUpDayInfo);

            var expectedBalanceUpDayInfo = new BalanceUpDayInfo(DateTime.Today, DateTime.Today);
            dynamicRepository.Get<BalanceUpDayInfo>(Arg.Any<ICriteria>()).Returns(expectedBalanceUpDayInfo);

            // Act
            var balanceUpDayInfo = balanceUpDayInfoService.GetBalanceUpDayInfo();

            // Assert
            Assert.AreEqual(balanceUpDayInfo, expectedBalanceUpDayInfo);
        }

        [Test]
        public void GetBalanceUpDayInfo_CachedIsNull_CallsGet()
        {
            // Arrange
            BalanceUpDayInfo cachedBalanceUpDayInfo = null;
            cacheService.Get<BalanceUpDayInfo>(BalanceUpDayKey).Returns(cachedBalanceUpDayInfo);

            // Act
            balanceUpDayInfoService.GetBalanceUpDayInfo();

            // Assert
            dynamicRepository.Received().Get<BalanceUpDayInfo>(Arg.Any<ICriteria>());
        }

        [Test]
        public void GetBalanceUpDayInfo_CachedIsNotNull_NotCallsGet()
        {
            // Arrange
            cacheService
                .Get<BalanceUpDayInfo>(BalanceUpDayKey)
                .Returns(new BalanceUpDayInfo(DateTime.Today, DateTime.Today));

            // Act
            balanceUpDayInfoService.GetBalanceUpDayInfo();

            // Assert
            dynamicRepository.DidNotReceive().Get<BalanceUpDayInfo>(Arg.Any<ICriteria>());
        }
    }
}