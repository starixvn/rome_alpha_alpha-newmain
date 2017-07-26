using Alpha.Main.AppResources;
using Alpha.Main.Models;
using Alpha.Main.Site.Controllers;
using Alpha.Main.Site.Requests.Otp;
using Alpha.SecurityCode.Web;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;
using System.Web.Mvc;

namespace Alpha.Main.Site.Tests.Controllers
{
    [TestFixture]
    public class OtpControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private OtpController otpController;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            otpController = new OtpController(mediator);
        }

        [Test]
        public void SetUpOtp_Always_CallsMeditorSendSetUpOtpQuery()
        {
            // Arrange
            var query = new SetUpOtpQuery(userSession);

            // Act
            otpController.SetUpOtp(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void VerifyOtp_IsEnabledOtpIsFalse_RedirectToVerifySecurityCode()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(false);
            var expectedUrl = "~/SecurityCode/VerifySecurityCode";

            // Act
            var result = otpController.VerifyOtp() as RedirectResult;

            // Assert
            Assert.AreEqual(expectedUrl, result.Url);
        }

        [Test]
        public void VerifyOtp_IsEnabledOtpIsTrue_ReturnsViewResult()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            // Act
            var result = otpController.VerifyOtp() as ViewResult;

            // Assert
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void VerifyOtp_IsEnabledOtpIsFalse_ReturnsNotAuthorizedResult()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(false);
            var command = new VerifyOtpCommand(userSession);

            // Act
            var result = otpController.VerifyOtp(command).Data as Result;

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void VerifyOtp_IsEnabledOtpIsTrue_CallsMediatorSendVerifyOtpCommand()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);
            var command = new VerifyOtpCommand(userSession);

            // Act
            otpController.VerifyOtp(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void SetUpOtp_Always_CallsMediatorSendSetUpOtpCommand()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);

            // Act
            otpController.SetUpOtp(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void ResetOtp_Always_CallsMediatorSendResetOtpQuery()
        {
            // Arrange
            var query = new ResetOtpQuery();

            // Act
            otpController.ResetOtp(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ResetOtp_Always_CallsMediatorSendResetOtpCommand()
        {
            // Arrange
            var command = new ResetOtpCommand(userSession);

            // Act
            otpController.ResetOtp(command);

            // Assert
            mediator.Received().Send(command);
        }

        [Test]
        public void ChangeOtpStatus_Always_CallsMediatorSendChangeOtpStatusCommand()
        {
            // Arrange
            var command = new ChangeOtpStatusCommand(userSession);

            // Act
            otpController.ChangeOtpStatus(command);

            // Assert
            mediator.Received().Send(command);
        }
    }
}
