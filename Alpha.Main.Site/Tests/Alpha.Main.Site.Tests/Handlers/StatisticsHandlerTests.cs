namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Statistics;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Home.Statistics;

    [TestFixture]
    public class StatisticsHandlerTests
    {
        private IUserSession userSession;
        private StatisticsService statisticsService;
        private StatisticsHandler statisticsHandler;

        [SetUp]
        public void SetUp()
        {
            statisticsService = Substitute.For<StatisticsService>();
            statisticsHandler = new StatisticsHandler(statisticsService);

            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);
        }

        [Test]
        public void HandleStatisticsQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            StatisticsQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => statisticsHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleTopWinningMembersQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            TopWinningMembersQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => statisticsHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleTopTurnoverMembersQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            TopTurnoverMembersQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => statisticsHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleNewMembersQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            NewMembersQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => statisticsHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleStatisticsQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var statistics = new Statistics(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            statisticsService
                .GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(statistics);

            var query = new StatisticsQuery(userSession);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetStatistics(Arg.Is<int>(custId => custId == query.CustId), Arg.Is<UserLevel>(us => us == query.UserLevel));
        }

        [Test]
        public void HandleTopWinningMembersQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var topWinningMembers = new List<TopWinningMember>();
            statisticsService
                .GetTopWinningMembers(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(topWinningMembers);

            var query = new TopWinningMembersQuery(userSession);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetTopWinningMembers(Arg.Is<int>(custId => custId == query.CustId), Arg.Is<UserLevel>(us => us == query.UserLevel));
        }

        [Test]
        public void HandleTopTurnoverMembersQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var topTurnoverMembers = new List<TopTurnoverMember>();
            statisticsService
                .GetTopTurnoverMembers(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(topTurnoverMembers);

            var query = new TopTurnoverMembersQuery(userSession);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetTopTurnoverMembers(Arg.Is<int>(custId => custId == query.CustId), Arg.Is<UserLevel>(us => us == query.UserLevel));
        }

        [Test]
        public void HandleNewMembersQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var newMembers = new List<NewMember>();
            statisticsService
                .GetNewMembers(Arg.Any<int>())
                .Returns(newMembers);

            var query = new NewMembersQuery(CustId.ValidId);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetNewMembers(Arg.Is<int>(custId => custId == query.CustId));
        }

        [Test]
        public void HandleStatisticsQuery_NotNullQuery_ReturnStatistics()
        {
            // Arrange
            var expectedStatistics = new Statistics(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            statisticsService
                .GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(expectedStatistics);

            var query = new StatisticsQuery(userSession);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedStatistics, viewModel.Statistics);
        }

        [Test]
        public void HandleTopWinningMembersQuery_NotNullQuery_ReturnTopWinningMembers()
        {
            // Arrange
            var expectedTopWinningMembers = new List<TopWinningMember>();
            statisticsService
                .GetTopWinningMembers(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(expectedTopWinningMembers);

            var query = new TopWinningMembersQuery(userSession);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedTopWinningMembers, viewModel.Members);
        }

        [Test]
        public void HandleTopTurnoverMembersQuery_NotNullQuery_ReturnTopTurnoverMembers()
        {
            // Arrange
            var expectedTopTurnoverMembers = new List<TopTurnoverMember>();
            statisticsService
                .GetTopTurnoverMembers(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(expectedTopTurnoverMembers);

            var query = new TopTurnoverMembersQuery(userSession);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedTopTurnoverMembers, viewModel.Members);
        }

        [Test]
        public void HandleNewMembersQuery_NotNullQuery_ReturnNewMembers()
        {
            // Arrange
            var expectedNewMembers = new List<NewMember>();
            statisticsService
                .GetNewMembers(Arg.Any<int>())
                .Returns(expectedNewMembers);

            var query = new NewMembersQuery(CustId.ValidId);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedNewMembers, viewModel.Members);
        }

        [Test]
        public void HandleStatisticsQuery_NotNullQuery_ReturnStatisticsViewModel()
        {
            // Arrange
            var statistics = new Statistics(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            statisticsService
                .GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>())
                .Returns(statistics);

            var query = new StatisticsQuery(userSession);

            var expectedStatisticsViewModel = new StatisticsViewModel(statistics, query.UserLevel);

            // Action
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.IsTrue(expectedStatisticsViewModel.UserLevel == viewModel.UserLevel);
        }
    }
}