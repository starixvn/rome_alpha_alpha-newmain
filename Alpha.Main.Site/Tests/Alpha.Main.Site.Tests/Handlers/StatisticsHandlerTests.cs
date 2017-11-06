#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Linq;
    using Constants;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Dashboard.Statistics;
    using Site.ViewModels.Dashboard.Statistics;
    using Sunplus.Agent.Model.Entities;

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
            userSession.SubPageAccess.Returns(new PageAccess());
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);
        }

        [Test]
        public void HandleStatisticsQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            StatisticsQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleTopWinningMembersQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            TopWinningMembersQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleTopTurnoverMembersQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            TopTurnoverMembersQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleNewMembersQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            NewMembersQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleStatisticsQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var statistics = FakeDataHelper.Create<Statistics>();
            statisticsService.GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(statistics);

            var query = new StatisticsQuery(userSession);

            // Act
            statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetStatistics(Arg.Is<int>(custId => custId == query.CustId), Arg.Is<UserLevel>(us => us == query.UserLevel));
        }

        [Test]
        public void HandleTopWinningMembersQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var topWinningMembers = FakeDataHelper.CreateMany<TopWinningMember>().ToList();
            statisticsService.GetTopWinningMembers(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(topWinningMembers);

            var query = new TopWinningMembersQuery(userSession);

            // Act
            statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetTopWinningMembers(Arg.Is<int>(custId => custId == query.CustId), Arg.Is<UserLevel>(us => us == query.UserLevel));
        }

        [Test]
        public void HandleTopTurnoverMembersQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var topTurnoverMembers = FakeDataHelper.CreateMany<TopTurnoverMember>().ToList();
            statisticsService.GetTopTurnoverMembers(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(topTurnoverMembers);

            var query = new TopTurnoverMembersQuery(userSession);

            // Act
            statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetTopTurnoverMembers(Arg.Is<int>(custId => custId == query.CustId), Arg.Is<UserLevel>(us => us == query.UserLevel));
        }

        [Test]
        public void HandleNewMembersQuery_NotNullQuery_CallGetStatisticsService()
        {
            // Arrange
            var newMembers = FakeDataHelper.CreateMany<NewMember>().ToList();
            statisticsService.GetNewMembers(Arg.Any<int>()).Returns(newMembers);

            var query = new NewMembersQuery(CustId.ValidId);

            // Act
            statisticsHandler.Handle(query);

            // Assert
            statisticsService.Received().GetNewMembers(Arg.Is<int>(custId => custId == query.CustId));
        }

        [Test]
        public void HandleStatisticsQuery_NotNullQuery_ReturnStatistics()
        {
            // Arrange
            var expectedStatistics = FakeDataHelper.Create<Statistics>();
            statisticsService.GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(expectedStatistics);

            var query = new StatisticsQuery(userSession);

            // Act
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedStatistics, viewModel.Statistics);
        }

        [Test]
        public void HandleTopWinningMembersQuery_NotNullQuery_ReturnTopWinningMembers()
        {
            // Arrange
            var expectedTopWinningMembers = FakeDataHelper.CreateMany<TopWinningMember>().ToList();
            statisticsService.GetTopWinningMembers(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(expectedTopWinningMembers);

            var query = new TopWinningMembersQuery(userSession);

            // Act
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedTopWinningMembers, viewModel.Members);
        }

        [Test]
        public void HandleTopTurnoverMembersQuery_NotNullQuery_ReturnTopTurnoverMembers()
        {
            // Arrange
            var expectedTopTurnoverMembers = FakeDataHelper.CreateMany<TopTurnoverMember>().ToList();
            statisticsService.GetTopTurnoverMembers(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(expectedTopTurnoverMembers);

            var query = new TopTurnoverMembersQuery(userSession);

            // Act
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedTopTurnoverMembers, viewModel.Members);
        }

        [Test]
        public void HandleNewMembersQuery_NotNullQuery_ReturnNewMembers()
        {
            // Arrange
            var expectedNewMembers = FakeDataHelper.CreateMany<NewMember>().ToList();
            statisticsService.GetNewMembers(Arg.Any<int>()).Returns(expectedNewMembers);

            var query = new NewMembersQuery(CustId.ValidId);

            // Act
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedNewMembers, viewModel.Members);
        }

        [Test]
        public void HandleStatisticsQuery_NotNullQuery_ReturnStatisticsViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            UrlHelperTestSetUp.SetUp();

            var statistics = FakeDataHelper.Create<Statistics>();
            statisticsService.GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(statistics);

            var query = new StatisticsQuery(userSession);

            var expectedStatisticsViewModel = new StatisticsViewModel(statistics, query.UserLevel, false);

            // Act
            var viewModel = statisticsHandler.Handle(query);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedStatisticsViewModel, viewModel).AreEqual);
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void HandleStatisticsQuery_ReturnStatisticsViewModel_WithCorrectReportPermission(bool viewReports, bool expectedResult)
        {
            // Arrange
            var pageAccess = new PageAccess();
            pageAccess.ViewReports = viewReports;
            userSession.SubPageAccess.Returns(pageAccess);

            var query = new StatisticsQuery(userSession);

            statisticsService.GetStatistics(Arg.Any<int>(), Arg.Any<UserLevel>()).Returns(FakeDataHelper.Create<Statistics>());

            // Act
            var result = statisticsHandler.Handle(query);

            // Assert
            Assert.IsTrue(expectedResult == result.CanViewReports);
        }
    }
}
