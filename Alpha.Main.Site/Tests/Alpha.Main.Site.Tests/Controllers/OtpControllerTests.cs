namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests.Otp;
    using Alpha.SecurityCode.Web;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class OtpControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private OtpController otpController;

        [SetUp]
        public void SetUp()
        {
            AppSettings.Settings = new NameValueCollection
            {
                ["OfferChatbotSites"] = "BEST-ODDS,733333"
            };
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns((byte)4);
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            otpController = new OtpController(mediator, userSession);
        }

        [Test]
        public async Task SetUpOtp_Always_CallsMeditorSendSetUpOtpQuery()
        {
            // Arrange
            var query = new SetUpOtpQuery(userSession);

            // Act
           await otpController.SetUpOtp(query);

            // Assert
            await mediator.Received().SendAsync(query);
        }

        //[Test]
        //public void VerifyOtp_IsEnabledOtpIsFalse_RedirectToVerifySecurityCode()
        //{
        //    // Arrange
        //    SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(false);
        //    var expectedUrl = "~/SecurityCode/VerifySecurityCode";

        //    // Act
        //    var result = otpController.VerifyOtp() as RedirectResult;

        //    // Assert
        //    Assert.AreEqual(expectedUrl, result.Url);
        //}

        //[Test]
        //public void VerifyOtp_IsEnabledOtpIsTrue_ReturnsViewResult()
        //{
        //    // Arrange
        //    SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

        //    // Act
        //    var result = otpController.VerifyOtp() as ViewResult;

        //    // Assert
        //    Assert.AreEqual(string.Empty, result.ViewName);
        //}

        [Test]
        public async Task VerifyOtp_IsEnabledOtpIsFalse_ReturnsNotAuthorizedResult()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(false);
            var command = new VerifyOtpCommand(userSession);

            // Act
            var result = (await otpController.VerifyOtp(command)).Data as Result;

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.NotAuthorized);
        }

        [Test]
        public async Task VerifyOtp_IsEnabledOtpIsTrue_CallsMediatorSendVerifyOtpCommand()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);
            var command = new VerifyOtpCommand(userSession);

            // Act
            await otpController.VerifyOtp(command);

            // Assert
            await mediator.Received().SendAsync(command);
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
        public async Task ResetOtp_Always_CallsMediatorSendResetOtpCommand()
        {
            // Arrange
            var command = new ResetOtpCommand(userSession);

            // Act
            await otpController.ResetOtp(command);

            // Assert
            await mediator.Received().SendAsync(command);
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

        [Test]
        public void VerifyOtpDifferenceDevice_Always_CallsMediatorSendVerifyOtpDifferentDeviceQuery()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var query = new VerifyOtpDifferentDeviceQuery(userSession);

            // Act
            otpController.VerifyOtpDifferenceDevice();

            // Assert
            mediator.Received().Send(Arg.Is<VerifyOtpDifferentDeviceQuery>(p => compareLogic.Compare(p, query).AreEqual));
        }

        [Test]
        public void RemoveOtp_Always_CallsMediatorSendRemoveOtpCommand()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var command = new RemoveOtpCommand(userSession);

            // Act
            otpController.RemoveOtp();

            // Assert
            mediator.Received().Send(Arg.Is<RemoveOtpCommand>(p => compareLogic.Compare(p, command).AreEqual));
        }
    }
}
