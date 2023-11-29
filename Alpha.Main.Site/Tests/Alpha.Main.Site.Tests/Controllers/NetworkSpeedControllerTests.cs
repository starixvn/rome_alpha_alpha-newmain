namespace Alpha.Main.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Requests.NetworkSpeed;
    using AlphaPlus.User.Models;

    [TestFixture]
    public class NetworkSpeedControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private NetworkSpeedController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();

            controller = new NetworkSpeedController(mediator, userSession);
        }

        [Test]
        public void Index_Always_CallsMediatorSendNetworkSpeedQuery()
        {
            // Arrange
            userSession.CustId.Returns(1);

            // Act
            controller.Index();

            // Assert
            mediator.Received()
                .Send(Arg.Is<NetworkSpeedQuery>(q => q.CustId == userSession.CustId));
        }

        [Test]
        public void SaveResult_Always_CallsMediatorSaveNetworkSpeedCommand()
        {
            // Arrange
            var command = new SaveNetworkSpeedCommand();

            // Act
            controller.SaveResult(command);

            // Assert
            mediator.Received()
                .Send(Arg.Is<SaveNetworkSpeedCommand>(q => ReferenceEquals(command, q)));
        }
    }
}