namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.AppResources;
    using Alpha.Main.Models;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Models.Request.SecurityCode;
    using Alpha.SecurityCode.Web;
    using Configurations;
    using MediatR;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class SecurityCodeControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private SecurityCodeController securityCodeController;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.IsForcedSecurityCode.Returns(true);
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();


            securityCodeController = new SecurityCodeController(mediator, userSession);
            AppSettings.Settings["SiteLabel"] = "MBC";
        }

        [Test]
        public void ChangeSecurityCodeFirstLogin_SecurityCodeIsNotNullOrWhiteSpace_RedirectToRoot()
        {
            // Arrange
            userSession.SecurityCode.Returns("1234aa");
            var query = new ForceChangeSecurityCodeQuery(userSession);
            var expectedUrl = "~/";

            // Act
            var result = securityCodeController.ChangeSecurityCodeFirstLogin(query) as RedirectResult;

            // Assert
            Assert.AreEqual(expectedUrl, result.Url);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangeSecurityCodeFirstLogin_SecurityCodeIsNullOrWhiteSpace_CallsMediatorSendForceChangeSecurityCodeQuery(string securityCode)
        {
            // Arrange
            userSession.SecurityCode.Returns(securityCode);
            var query = new ForceChangeSecurityCodeQuery(userSession);

            // Act
            securityCodeController.ChangeSecurityCodeFirstLogin(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ChangeSecurityCodeFirstLogin_SecurityCodeIsNotNullOrWhiteSpace_ReturnsNotAutorizedResult()
        {
            // Arrange
            userSession.SecurityCode.Returns("1234aa");
            var query = new ChangeSecurityCodeFirstLoginCommand(userSession);

            // Act
            var result = securityCodeController.ChangeSecurityCodeFirstLogin(query).Data as Result;

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.NotAuthorized);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangeSecurityCodeFirstLogin_SecurityCodeIsNullOrWhiteSpace_CallsMediatorSendChangeSecurityCodeFirstLoginCommand(string securityCode)
        {
            // Arrange
            userSession.SecurityCode.Returns(securityCode);
            var query = new ChangeSecurityCodeFirstLoginCommand(userSession);

            // Act
            securityCodeController.ChangeSecurityCodeFirstLogin(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ChangeSecurityCodeByForcing_Always_CallsMediatorSendForceChangeSecurityCodeQuery()
        {
            // Arrange
            var query = new ForceChangeSecurityCodeQuery(userSession);

            // Act
            securityCodeController.ChangeSecurityCodeByForcing(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ChangeSecurityCodeByForcing_Always_CallsMediatorSendChangeSecurityCodeByForcingCommand()
        {
            // Arrange
            var query = new ChangeSecurityCodeByForcingCommand(userSession);

            // Act
            securityCodeController.ChangeSecurityCodeByForcing(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ChangeUserOwnSecurityCode_Always_CallsMediatorSendSecurityCodeQuery()
        {
            // Arrange
            var query = new SecurityCodeQuery(userSession);

            // Act
            securityCodeController.ChangeUserOwnSecurityCode(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ChangeUserOwnSecurityCode_Always_CallsMediatorSendChangeUserOwnSecurityCodeCommand()
        {
            // Arrange
            var query = new ChangeUserOwnSecurityCodeCommand(userSession);

            // Act
            securityCodeController.ChangeUserOwnSecurityCode(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ResetSecurityCode_Always_CallsMediatorSendSecurityCodeQuery()
        {
            // Arrange
            var query = new SecurityCodeQuery(userSession);

            // Act
            securityCodeController.ResetSecurityCode(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void ResetSecurityCode_Always_CallsMediatorSendResetSecurityCodeCommand()
        {
            // Arrange
            var query = new ResetSecurityCodeCommand(userSession);

            // Act
            securityCodeController.ResetSecurityCode(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void VerifySecurityCode_IsEnabledOtpIsFalse_CallsMediatorSendSecurityCodeQuery()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(false);

            var query = new SecurityCodeQuery(userSession);

            // Act
            securityCodeController.VerifySecurityCode(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void VerifySecurityCode_IsEnabledOtpIsTrue_RedirectToVerifyOtp()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);
            var expectedUrl = "~/Otp/VerifyOtp";

            var query = new SecurityCodeQuery(userSession);

            // Act
            var result = securityCodeController.VerifySecurityCode(query) as RedirectResult;

            // Assert
            Assert.AreEqual(expectedUrl, result.Url);
        }

        [Test]
        public void VerifySecurityCode_IsEnabledOtpIsFalse_CallsMediatorSendVerifySecurityCodeCommand()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(false);

            var query = new VerifySecurityCodeCommand(userSession);

            // Act
            securityCodeController.VerifySecurityCode(query);

            // Assert
            mediator.Received().Send(query);
        }

        [Test]
        public void VerifySecurityCode_IsEnabledOtpIsTrue_ReturnsNotAuthorizedResult()
        {
            // Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            var query = new VerifySecurityCodeCommand(userSession);

            // Act
            var result = securityCodeController.VerifySecurityCode(query).Data as Result;

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void LearnMore_Always_ReturnsCorectView()
        {
            // Arrange
            userSession.Language.Returns("en-Us");
            var expectedViewName = "~/Views/SecurityCode/LearnMore/SecurityCode_en-Us.cshtml";

            // Act
            var viewResult = securityCodeController.LearnMore() as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, viewResult.ViewName);
        }

        ////[Test]
        ////public void Languages_Always_CallsMediatorSendAllLanguagesQuery()
        ////{
        ////    //Arrange
        ////    var query = new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese);

        ////    // Act
        ////    securityCodeController.Languages();

        ////    // Assert
        ////    mediator.Received().Send(
        ////        Arg.Is<AllLanguagesQuery>(
        ////            param => param.LanguageCode == query.LanguageCode
        ////            && param.DisabledVietnamese == query.DisabledVietnamese));
        ////}
    }
}
