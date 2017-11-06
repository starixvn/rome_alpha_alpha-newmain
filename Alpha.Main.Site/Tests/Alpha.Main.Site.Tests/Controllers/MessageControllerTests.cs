namespace Alpha.Main.Site.Tests.Controllers
{
    using System;
    using Configurations;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Requests.Message;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class MessageControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private MessageController messageController;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();

            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);

            messageController = new MessageController(mediator, userSession);
        }

        [Test]
        public void Index_Always_CallsMediatorSendMessageQuery()
        {
            // Act
            messageController.Index();

            // Assert
            mediator.Received().Send(Arg.Any<MessageQuery>());
        }

        [Test]
        public void Messages_Always_CallsMediatorSendMessageQuery()
        {
            var compareLogic = new CompareLogic();
            var startDate = DateTime.Today.AddDays(-3);
            var endDate = DateTime.Today;

            // Arrange
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                startDate,
                endDate,
                AppSettings.LocalTest);

            // Act
            messageController.Messages(startDate, endDate);

            // Assert
            mediator.Received().Send(Arg.Is<MessageQuery>(p => compareLogic.Compare(p, query).AreEqual));
        }
    }
}
