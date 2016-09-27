namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StatisticsServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private BalanceUpDayInfoService balanceUpDateInfoService;
        private StatisticsServiceImpl statisticsService;
        private UserLevel userLevel;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            balanceUpDateInfoService = Substitute.For<BalanceUpDayInfoService>();
            balanceUpDateInfoService.GetBalanceUpDayInfo().Returns(new BalanceUpDayInfo(DateTime.Today, DateTime.Today));

            userLevel = Enumeration.FromValue<UserLevel>(2);

            statisticsService = new StatisticsServiceImpl(balanceUpDateInfoService, dynamicRepository);
        }

        [Test]
        public void GetStatistics_InvalidCustId_ReturnNull()
        {
            // Action
            var statistics = statisticsService.GetStatistics(0, userLevel);

            // Assert
            Assert.AreEqual(statistics, null);
        }

        [Test]
        public void GetStatistics_ValidCustId_ReturnStatisticsInfo()
        {
            // Arrange
            var expectedStatistics = new Statistics(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            dynamicRepository.Get<Statistics>(Arg.Any<object>()).Returns(expectedStatistics);

            // Action
            var statistics = statisticsService.GetStatistics(1, userLevel);

            // Assert
            Assert.AreEqual(statistics, expectedStatistics);
        }

        [Test]
        public void GetTopWinningMembers_InvalidCustId_ReturnEmptyList()
        {
            // Action
            var topWinningMembers = statisticsService.GetTopWinningMembers(0, userLevel);

            // Assert
            Assert.AreEqual(topWinningMembers, new List<TopWinningMember>());
        }

        [Test]
        public void GetTopWinningMembers_ValidCustId_ReturnTopWinningMembers()
        {
            // Arrange
            var expectedTopWinningMembers = new List<TopWinningMember>();
            expectedTopWinningMembers.Add(new TopWinningMember(1, "user", 1));

            dynamicRepository.Fetch<TopWinningMember>(Arg.Any<object>()).Returns(expectedTopWinningMembers);

            // Action
            var topWinningMembers = statisticsService.GetTopWinningMembers(0, userLevel);

            // Assert
            Assert.AreEqual(topWinningMembers, expectedTopWinningMembers);
        }

        [Test]
        public void GetTopTurnoverMembers_InvalidCustId_ReturnEmptyList()
        {
            // Action
            var topTurnoverMembers = statisticsService.GetTopTurnoverMembers(0, userLevel);

            // Assert
            Assert.AreEqual(topTurnoverMembers, new List<TopTurnoverMember>());
        }

        [Test]
        public void GetTopTurnoverMembers_ValidCustId_ReturnTopTurnoverMembers()
        {
            // Arrange
            var expectedTopTurnoverMembers = new List<TopTurnoverMember>();
            expectedTopTurnoverMembers.Add(new TopTurnoverMember(1, "user", 1));

            dynamicRepository.Fetch<TopTurnoverMember>(Arg.Any<object>()).Returns(expectedTopTurnoverMembers);

            // Action
            var topTurnoverMembers = statisticsService.GetTopTurnoverMembers(0, userLevel);

            // Assert
            Assert.AreEqual(topTurnoverMembers, expectedTopTurnoverMembers);
        }

        [Test]
        public void GetNewMembers_InvalidCustId_ReturnEmptyList()
        {
            // Action
            var newMembers = statisticsService.GetNewMembers(0);

            // Assert
            Assert.AreEqual(newMembers, new List<NewMember>());
        }

        [Test]
        public void GetNewMembers_ValidCustId_ReturnNewMembers()
        {
            // Arrange
            var expectedNewMembers = new List<NewMember>();
            expectedNewMembers.Add(new NewMember(1, "user", DateTime.Today, 1));

            dynamicRepository.Fetch<NewMember>(Arg.Any<object>()).Returns(expectedNewMembers);

            // Action
            var newMembers = statisticsService.GetNewMembers(0);

            // Assert
            Assert.AreEqual(newMembers, expectedNewMembers);
        }
    }
}