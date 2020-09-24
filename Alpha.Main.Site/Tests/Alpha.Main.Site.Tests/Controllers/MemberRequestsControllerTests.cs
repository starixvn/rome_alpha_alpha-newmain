namespace Alpha.Main.Site.Tests.Controllers
{
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests.Dashboard.MemberRequests;
    using Alpha.Web.Models;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;
    using System.Web.Mvc;

    [TestFixture]
    public class MemberRequestsControllerTests
    {
        private MemberRequestsController memberRequestsController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);

            memberRequestsController = new MemberRequestsController(mediator, userSession);
        }

        [TestCase(RoleId.Agent)]
        [TestCase(RoleId.Master)]
        [TestCase(RoleId.Super)]
        public void Guidelines_Always_ReturnsCorrectViewName(byte roleId)
        {
            // Arrange
            userSession.Language.Returns("en-us");
            userSession.RoleId.Returns(roleId);
            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            var expectedViewName = $"~/Views/MemberRequests/Guidelines/en-us/{custLevel.DisplayName}/Index.cshtml";

            // Act
            var actualView = memberRequestsController.Guidelines() as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, actualView.ViewName);
        }

        [Test]
        public void Index_Always_CallsMediatorSendMemberRequestsQuery()
        {
            // Arrange
            var query = new MemberRequestsQuery(userSession);

            // Act
            memberRequestsController.Index();

            // Assert
            mediator.Received().Send(
                Arg.Is<MemberRequestsQuery>(
                    param => param.UserSession == query.UserSession
                    && param.UserLevel == query.UserLevel));
        }
    }
}
