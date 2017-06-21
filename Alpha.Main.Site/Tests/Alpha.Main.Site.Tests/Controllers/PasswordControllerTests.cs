using Alpha.Main.Site.Controllers;
using Alpha.Main.Site.Requests.Password;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Controllers
{
    [TestFixture]
    public class PasswordControllerTests
    {
        private PasswordController passwordController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            passwordController = new PasswordController(mediator);
        }

        [Test]
        public void ChangePassword_Always_CallsMediatorSendChangePasswordQuery()
        {
            // Arrange
            var query = new ChangePasswordQuery(userSession);

            // Act
            passwordController.ChangePassword(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ForceChangePassword_Always_CallsMediatorSendForceChangePasswordQuery()
        {
            // Arrange
            var query = new ForceChangePasswordQuery(userSession);

            // Act
            passwordController.ForceChangePassword(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ResetMainAccountPassword_Always_CallsMediatorSendResetMainAccountPasswordQuery()
        {
            // Arrange
            var query = new ResetMainAccountPasswordQuery();

            // Act
            passwordController.ResetMainAccountPassword(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ResetSubAccountPassword_Always_CallsMediatorSendResetSubAccountPasswordQuery()
        {
            // Arrange
            var query = new ResetSubAccountPasswordQuery();

            // Act
            passwordController.ResetSubAccountPassword(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ChangePassword_Always_CallsMediatorSendChangePasswordCommand()
        {
            // Arrange
            var command = new ChangePasswordCommand(userSession, true);

            // Act
            passwordController.ChangePassword(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void ForceChangePassword_Always_CallsMediatorSendChangePasswordCommand()
        {
            // Arrange
            var command = new ChangePasswordCommand(userSession, true);

            // Act
            passwordController.ForceChangePassword(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void ResetMainAccountPassword_Always_CallsMediatorSendResetMainAccountPasswordCommand()
        {
            // Arrange
            var command = new ResetMainAccountPasswordCommand(userSession, true);

            // Act
            passwordController.ResetMainAccountPassword(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void ResetSubAccountPassword_Always_CallsMediatorSendResetSubAccountPasswordCommand()
        {
            // Arrange
            var command = new ResetSubAccountPasswordCommand(userSession);

            // Act
            passwordController.ResetSubAccountPassword(command);

            // Assert
            mediator.Received().Send(command);
        }
    }
}
