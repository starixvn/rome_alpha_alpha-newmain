namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests.Password;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

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
            userSession.IsPasswordExpired.Returns(true);
            passwordController = new PasswordController(mediator, userSession);
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
            var query = new ResetMainAccountPasswordQuery
            {
                CustId = 1,
                CustRoleId = RoleId.Agent,
                IsCasinoSync = true,
                Username = "UserName"
            };

            // Act
            passwordController.ResetMainAccountPassword(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ResetMainAccountPassword_QueryIsInvalid_RedirectToErrorPage()
        {
            // Arrange
            var query = new ResetMainAccountPasswordQuery
            {
                CustId = 1,
                CustRoleId = 0,
                IsCasinoSync = true,
                Username = "UserName"
            };

            // Act
            var result = (RedirectToRouteResult)passwordController.ResetMainAccountPassword(query);

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Error", result.RouteValues["controller"]);
        }

        [Test]
        public void ResetSubAccountPassword_Always_CallsMediatorSendResetSubAccountPasswordQuery()
        {
            // Arrange
            var query = new ResetSubAccountPasswordQuery
            {
                SubAccountId = 1,
                Username = "UserName"
            };

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
        public void ResetMainAccountPassword_ValidParamter_CallsMediatorSendResetMainAccountPasswordCommand()
        {
            // Arrange
            var command = new ResetMainAccountPasswordCommand(userSession, true);
            command.CustRoleId = 2;
            // Act
            passwordController.ResetMainAccountPassword(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void ResetMainAccountPassword_InvalidParamter_NotCallsMediatorSendResetMainAccountPasswordCommand()
        {
            // Arrange
            var command = new ResetMainAccountPasswordCommand(userSession, true);
            command.CustRoleId = 0;
            // Act
            passwordController.ResetMainAccountPassword(command);

            // Assert
            mediator.DidNotReceive().Send(command);
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
