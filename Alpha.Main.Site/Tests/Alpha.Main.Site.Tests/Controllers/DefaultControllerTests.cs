namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using NSubstitute;
    using NUnit.Framework;
    using SecurityCode.Services;
    using Site.Controllers;
    using Site.Requests;
    using Site.Requests.Otp;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    [TestFixture]
    public class DefaultControllerTests
    {
        private IUserSession userSession;
        private IMediator mediator;
        private SecurityCodeChecker securityCodeChecker;
        private NicknameChecker nicknameChecker;
        private DefaultController defaultController;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns("en");
            userSession.RoleId.Returns(RoleId.Agent);

            mediator = Substitute.For<IMediator>();
            securityCodeChecker = Substitute.For<SecurityCodeChecker>();
            nicknameChecker = Substitute.For<NicknameChecker>();

            defaultController = new DefaultController(userSession, mediator, securityCodeChecker, nicknameChecker);
        }

        [Test]
        public void Index_Always_CallsMediatorSendBrowserQuery()
        {
            // Arrange
            var query = new BrowserQuery(userSession);
            mediator.Send(Arg.Any<BrowserQuery>()).Returns(new BrowserViewModel());

            // Act
            defaultController.Index();

            // Assert
            mediator.Received().Send(Arg.Is<BrowserQuery>(q => q.UserLevelId == query.UserLevelId));
        }

        [Test]
        public void Index_UnsupportedBrowser_ReturnsUnSupportedBrowsersView()
        {
            // Arrange
            var expectedViewName = "UnsupportedBrowsers";

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = true
                });

            // Act
            var result = defaultController.Index() as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
        }

        [TestCase(false, true)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void Index_UserIsNotLoggedOrPasswordIsExpired_RedirectToForceChangePasswordPath(bool isLogged, bool isPasswordExpired)
        {
            // Arrange
            var forceChangePasswordPath = "~/Password/ForceChangePassword";

            userSession.IsLogged.Returns(isLogged);
            userSession.IsPasswordExpired.Returns(isPasswordExpired);

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = false
                });

            // Act
            var result = defaultController.Index() as RedirectResult;

            // Assert
            Assert.AreEqual(forceChangePasswordPath, result.Url);
        }

        [Test]
        public void Index_VerifyDifferenceDevideCommand_CallsMediatorSendCheckDifferenceDeviceCommand()
        {
            // Arrange
            userSession.IsLogged.Returns(true);
            userSession.IsPasswordExpired.Returns(false);

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = false
                });

            // Act
            defaultController.Index();

            // Assert
            mediator.Received().Send(Arg.Is<CheckDifferenceDeviceCommand>(
                p => p.CustId == userSession.CustId
                && p.SubAccountId == userSession.SubAccId));
        }

        [Test]
        public void Index_VerifyDifferenceDevideCommand_RedirectToOtpVerifyDifferenceDevicePath()
        {
            // Arrange
            userSession.IsLogged.Returns(true);
            userSession.IsPasswordExpired.Returns(false);

            var otpVerifyDifferenceDevicePath = "~/otp/VerifyOtpDifferenceDevice";

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = false
                });

            mediator.Send(Arg.Any<CheckDifferenceDeviceCommand>()).Returns(true);

            // Act
            var result = defaultController.Index() as RedirectResult;

            // Assert
            Assert.AreEqual(otpVerifyDifferenceDevicePath, result.Url);
        }

        [Test]
        public void Index_IsForcedChangeSecurityCode_RedirectToSecurityCodePath()
        {
            // Arrange
            userSession.IsLogged.Returns(true);
            userSession.IsPasswordExpired.Returns(false);

            var securityCodePath = "~/SecurityCode/Index";

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = false
                });

            mediator.Send(Arg.Any<CheckDifferenceDeviceCommand>()).Returns(false);
            securityCodeChecker.IsForcedChangeSecurityCode().Returns(true);

            // Act
            var result = defaultController.Index() as RedirectResult;

            // Assert
            Assert.AreEqual(securityCodePath, result.Url);
        }

        [Test]
        public void Index_IsNotForcedChangeSecurityCode_CallsMediatorSendIndexQuery()
        {
            // Arrange
            var compareLogic = new CompareLogic();

            userSession.IsLogged.Returns(true);
            userSession.IsPasswordExpired.Returns(false);
            userSession.SubPageAccess.Returns(new PageAccess());

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = false
                });

            mediator.Send(Arg.Any<CheckDifferenceDeviceCommand>()).Returns(false);

            securityCodeChecker.IsForcedChangeSecurityCode().Returns(false);
            securityCodeChecker.IsEnableSecurityCode().Returns(true);

            var query = new IndexQuery(userSession, securityCodeChecker.IsEnableSecurityCode(), false);

            // Act
            defaultController.Index();

            // Assert
            mediator.Received().Send(
                Arg.Is<IndexQuery>(message => compareLogic.Compare(message, query).AreEqual));
        }

        [Test]
        public void HeaderHotMsg_Always_CallsMediatorSendHotMessagesQuery()
        {
            // Assert
            var query = new HotMessagesQuery
            {
                Language = userSession.Language,
                RoleId = userSession.RoleId
            };

            // Act
            defaultController.HeaderHotMsg();

            // Assert
            mediator.Received().Send(Arg.Is<HotMessagesQuery>(
                q => q.Language == query.Language
                    && q.RoleId == query.RoleId));
        }

        [Test]
        public void MenuBar_Always_CallsMediatorSendMenuBarQuery()
        {
            // Arrange
            var fakeRequest = Substitute.For<HttpRequestBase>();
            var fakeContext = Substitute.For<HttpContextBase>();
            fakeRequest.Headers.Returns(new WebHeaderCollection { { "X-Requested-With", "XMLHttpRequest" } });
            fakeContext.Request.Returns(fakeRequest);
            defaultController.ControllerContext = new ControllerContext(fakeContext, new System.Web.Routing.RouteData(), defaultController);

            // Act
            defaultController.MenuBar();

            // Assert
            mediator.Received().Send(Arg.Any<MenuBarQuery>());
        }
    }
}
