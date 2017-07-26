using Alpha.Main.Constants;
using Alpha.Main.Site.Controllers;
using Alpha.Main.Site.Requests.Dashboard.Statistics;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;
using System.Web.Mvc;

namespace Alpha.Main.Site.Tests.Controllers
{
    [TestFixture]
    public class StatisticsControllerTests
    {
        private StatisticsController statisticsController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);

            statisticsController = new StatisticsController(mediator, userSession);
        }

        [Test]
        public void Index_Always_MediatorSendStatisticsQuery()
        {
            // Act
            statisticsController.Index();

            // Assert
            mediator.Received().Send(Arg.Any<StatisticsQuery>());
        }

        [Test]
        public void TopWinningMembers_Always_CallsMediatorSendTopWinningMembersQuery()
        {
            // Act
            statisticsController.TopWinningMembers();

            // Assert
            mediator.Received().Send(Arg.Any<TopWinningMembersQuery>());
        }

        [Test]
        public void TopWinningMembers_Always_ReturnsCorrectPartialViewName()
        {
            // Arrange
            var expectedPartialViewName = "TopWinningMembers";

            // Act
            var actualPartialView = statisticsController.TopWinningMembers() as PartialViewResult;

            // Assert
            Assert.AreEqual(expectedPartialViewName, actualPartialView.ViewName);
        }

        [Test]
        public void TopTurnoverMembers_Always_CallsMediatorSendTopTurnoverMembersQuery()
        {
            // Act
            statisticsController.TopTurnoverMembers();

            // Assert
            mediator.Received().Send(Arg.Any<TopTurnoverMembersQuery>());
        }

        [Test]
        public void TopTurnoverMembers_Always_ReturnsCorrectPartialViewName()
        {
            // Arrange
            var expectedPartialViewName = "TopTurnoverMembers";

            // Act
            var actualPartialView = statisticsController.TopTurnoverMembers() as PartialViewResult;

            // Assert
            Assert.AreEqual(expectedPartialViewName, actualPartialView.ViewName);
        }

        [Test]
        public void NewMembers_Always_CallsMediatorSendNewMembersQuery()
        {
            // Act
            statisticsController.NewMembers();

            // Assert
            mediator.Received().Send(Arg.Any<NewMembersQuery>());
        }

        [Test]
        public void NewMembers_Always_ReturnsCorrectPartialViewName()
        {
            // Arrange
            var expectedPartialViewName = "NewMembers";

            // Act
            var actualPartialView = statisticsController.NewMembers() as PartialViewResult;

            // Assert
            Assert.AreEqual(expectedPartialViewName, actualPartialView.ViewName);
        }
    }
}
