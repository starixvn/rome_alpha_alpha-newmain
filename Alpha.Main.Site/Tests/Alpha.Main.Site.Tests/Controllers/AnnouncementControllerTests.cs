using Alpha.Main.Site.Controllers;
using Alpha.Main.Site.Models.Requests.Announcement;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Controllers
{
    [TestFixture]
    public class AnnouncementControllerTests
    {
        private AnnouncementController announcementController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            announcementController = new AnnouncementController(mediator);
        }

        [Test]
        public void HotAnnouncement_Always_CallsMediatorSendHotAnnouncementQuery()
        {
            // Arrange
            var message = new HotAnnouncementQuery(null, userSession, string.Empty, string.Empty, new string[] { }, null);

            // Act
            announcementController.HotAnnouncement(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}
