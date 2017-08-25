namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fanex.Data.Repository;
    using Main.DataAccesses;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class StatisticsServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private BalanceUpDayInfoService balanceUpDateInfoService;
        private StatisticsDataAccess statisticsDataAccess;
        private StatisticsServiceImpl statisticsService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            statisticsDataAccess = Substitute.For<StatisticsDataAccess>();

            balanceUpDateInfoService = Substitute.For<BalanceUpDayInfoService>();
            balanceUpDateInfoService
                .GetBalanceUpDayInfo()
                .Returns(new BalanceUpDayInfo(DateTime.Today, DateTime.Today));

            statisticsService = new StatisticsServiceImpl(balanceUpDateInfoService, dynamicRepository, statisticsDataAccess);
        }

        [Test]
        public void GetStatistics_InvalidCustId_ReturnNull()
        {
            // Act
            var statistics = statisticsService.GetStatistics(CustId.InvalidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(statistics, null);
        }

        [Test]
        public void GetStatistics_ValidCustId_ReturnStatisticsInfo()
        {
            // Arrange
            var expectedStatistics = FakeDataHelper.Create<Statistics>();

            statisticsDataAccess.GetStatistics(Arg.Any<ICriteria>(), Arg.Any<UserLevel>()).Returns(expectedStatistics);

            // Act
            var statistics = statisticsService.GetStatistics(CustId.ValidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(statistics, expectedStatistics);
        }

        [Test]
        public void GetTopWinningMembers_InvalidCustId_ReturnEmptyList()
        {
            // Act
            var topWinningMembers = statisticsService.GetTopWinningMembers(CustId.InvalidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(topWinningMembers, null);
        }

        [Test]
        public void GetTopWinningMembers_ValidCustId_ReturnTopWinningMembers()
        {
            // Arrange
            var expectedTopWinningMembers = FakeDataHelper.CreateMany<TopWinningMember>().ToList();

            statisticsDataAccess.GetTopWinningMembers(Arg.Any<ICriteria>()).Returns(expectedTopWinningMembers);

            // Act
            var topWinningMembers = statisticsService.GetTopWinningMembers(CustId.ValidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(topWinningMembers, expectedTopWinningMembers);
        }

        [Test]
        public void GetTopTurnoverMembers_InvalidCustId_ReturnEmptyList()
        {
            // Act
            var topTurnoverMembers = statisticsService.GetTopTurnoverMembers(CustId.InvalidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(topTurnoverMembers, null);
        }

        [Test]
        public void GetTopTurnoverMembers_ValidCustId_ReturnTopTurnoverMembers()
        {
            // Arrange
            var expectedTopTurnoverMembers = FakeDataHelper.CreateMany<TopTurnoverMember>().ToList();

            statisticsDataAccess.GetTopTurnoverMembers(Arg.Any<ICriteria>()).Returns(expectedTopTurnoverMembers);

            // Act
            var topTurnoverMembers = statisticsService.GetTopTurnoverMembers(CustId.ValidId, UserLevel.Agent);

            // Assert
            Assert.AreEqual(topTurnoverMembers, expectedTopTurnoverMembers);
        }

        [Test]
        public void GetNewMembers_InvalidCustId_ReturnEmptyList()
        {
            // Act
            var newMembers = statisticsService.GetNewMembers(CustId.InvalidId);

            // Assert
            Assert.AreEqual(newMembers, new List<NewMember>());
        }

        [Test]
        public void GetNewMembers_ValidCustId_ReturnNewMembers()
        {
            // Arrange
            var expectedNewMembers = FakeDataHelper.CreateMany<NewMember>().ToList();

            dynamicRepository.Fetch<NewMember>(Arg.Any<CriteriaBase>()).Returns(expectedNewMembers);

            // Act
            var newMembers = statisticsService.GetNewMembers(CustId.ValidId);

            // Assert
            Assert.AreEqual(newMembers, expectedNewMembers);
        }
    }
}
