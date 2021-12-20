namespace Alpha.Main.Tests.Services
{
    using Alpha.Web.Models;
    using Constants;
    using Fanex.Data.Repository;
    using Main.Models.BalanceInfo;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using TestData.Helper;

    [TestFixture]
    public class BalanceInfoServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private BalanceUpDayInfoService balanceUpDayInfoService;
        private BalanceInfoServiceImpl balanceInfoService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            balanceUpDayInfoService = Substitute.For<BalanceUpDayInfoService>();
            balanceUpDayInfoService.GetBalanceUpDayInfo().Returns(new BalanceUpDayInfo(DateTime.Today, DateTime.Today));

            balanceInfoService = new BalanceInfoServiceImpl(balanceUpDayInfoService, dynamicRepository);
        }

        [Test]
        public void GetBalanceInfo_InvalidCustId_ReturnNull()
        {
            // Act
            var balanceInfo = balanceInfoService.GetBalanceInfo(CustId.InvalidId, UserLevel.Agent);

            // Assert
            Assert.IsNull(balanceInfo);
        }

        [Test]
        public void GetBalanceInfo_ValidCustId_ReturnBalanceInfo()
        {
            // Arrange
            var expectedBalanceInfo = FakeDataHelper.Create<BalanceInfo>();
            dynamicRepository.Get<BalanceInfo>(Arg.Any<ICriteria>()).Returns(expectedBalanceInfo);

            // Act
            var balanceInfo = balanceInfoService.GetBalanceInfo(CustId.ValidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(balanceInfo, expectedBalanceInfo);
        }
    }
}