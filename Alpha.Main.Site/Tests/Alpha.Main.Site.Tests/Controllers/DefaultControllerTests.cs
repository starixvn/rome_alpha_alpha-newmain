namespace Alpha.Main.Site.Tests.Controllers
{
    using Alpha.Main.Services.Impl;
    using Alpha.Web.Models;
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
    using Sunplus.Agent.Model.Entities;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Web.Helpers;

    [TestFixture]
    public class DefaultControllerTests
    {
        private IUserSession userSession;
        private IMediator mediator;
        private SecurityCodeChecker securityCodeChecker;
        private NicknameChecker nicknameChecker;
        private DefaultController defaultController;
        private IUpdateUsageLanguageService updateUsageLanguageService;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns("en");
            userSession.RoleId.Returns(RoleId.Agent);

            mediator = Substitute.For<IMediator>();
            securityCodeChecker = Substitute.For<SecurityCodeChecker>();
            nicknameChecker = Substitute.For<NicknameChecker>();
            updateUsageLanguageService = Substitute.For<IUpdateUsageLanguageService>();

            defaultController = new DefaultController(userSession, mediator, securityCodeChecker, nicknameChecker, updateUsageLanguageService);
        }

        [Test]
        public async Task Index_Always_CallsMediatorSendBrowserQuery()
        {
            // Arrange
            var query = new BrowserQuery(userSession);
            mediator.Send(Arg.Any<BrowserQuery>()).Returns(new BrowserViewModel());

            // Act
            await defaultController.Index();

            // Assert
            mediator.Received().Send(Arg.Is<BrowserQuery>(q => q.UserLevelId == query.UserLevelId));
        }

        [Test]
        public async Task Index_UnsupportedBrowser_ReturnsUnSupportedBrowsersView()
        {
            // Arrange
            var expectedViewName = "UnsupportedBrowsers";

            mediator.Send(Arg.Any<BrowserQuery>()).Returns(
                new BrowserViewModel
                {
                    IsUnsupportedBrowser = true
                });

            // Act
            var result = (await defaultController.Index()) as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
        }

        [Test]
        public async Task Index_VerifyDifferenceDevideCommand_CallsMediatorSendCheckDifferenceDeviceCommand()
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
            await defaultController.Index();

            // Assert
            await mediator.Received().SendAsync(Arg.Is<CheckDifferenceDeviceCommand>(
                p => p.CustId == userSession.CustId
                && p.SubAccountId == userSession.SubAccId));
        }

        [Test]
        public async Task Index_VerifyDifferenceDevideCommand_RedirectToOtpVerifyDifferenceDevicePath()
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

            mediator.SendAsync(Arg.Any<CheckDifferenceDeviceCommand>()).Returns(Task.FromResult(true));

            // Act
            var result = await (defaultController.Index()) as RedirectResult;

            // Assert
            Assert.AreEqual(otpVerifyDifferenceDevicePath, result.Url);
        }

        [Test]
        public async Task Index_IsNotForcedChangeSecurityCode_CallsMediatorSendIndexQuery()
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

            mediator.SendAsync(Arg.Any<CheckDifferenceDeviceCommand>()).Returns(Task.FromResult(false));

            securityCodeChecker.IsForcedChangeSecurityCode().Returns(false);
            securityCodeChecker.IsEnableSecurityCode().Returns(true);

            var query = new IndexQuery(userSession, securityCodeChecker.IsEnableSecurityCode(), false, nicknameChecker);

            // Act
             await defaultController.Index();

            // Assert
            mediator.Received().SendAsync(
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
