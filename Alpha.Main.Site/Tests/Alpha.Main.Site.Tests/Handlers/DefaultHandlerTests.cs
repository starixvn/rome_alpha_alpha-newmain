namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Alpha.Main.Configurations;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.Main.Services;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Helpers;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Requests;
    using Site.ViewModels;
    using TelegramBot.Client.Services;

    [TestFixture]
    public class DefaultHandlerTests
    {
        private const string PasswordHashSessionKey = "ph";
        private const string SawBrowserWarningPageSessionKey = "DefaultHandler.SawBrowserWarningPageSessionKey";
        private SessionManager sessionManager;
        private DefaultHandler defaultHandler;
        private IUserSession userSession;
        private BrowserHelper browserHelper;
        private Func<string> getDefaultUrl;
        private Func<string, string> encodeDataFunction;
        private LogoutService logoutService;
        private ProfilePictureServices customerProfileService;
        private ActorProfilePictureModel profilePicture;
        private NicknameChecker nicknameChecker;
        private ISettingService telegramSettingService;
        private TransferService transferService;

        [SetUp]
        public void SetUp()
        {
            AppSettings.Settings["UMAnnouncementPopup:ClientIntervalInMinutes"] = "60";
            browserHelper = Substitute.For<BrowserHelper>();
            sessionManager = Substitute.For<SessionManager>();
            customerProfileService = Substitute.For<ProfilePictureServices>();
            getDefaultUrl = Substitute.For<Func<string>>();
            encodeDataFunction = Substitute.For<Func<string, string>>();
            logoutService = Substitute.For<LogoutService>();
            telegramSettingService = Substitute.For<ISettingService>();
            transferService = Substitute.For<TransferService>();
            defaultHandler = new DefaultHandler(sessionManager, browserHelper, logoutService, getDefaultUrl, encodeDataFunction, customerProfileService, telegramSettingService, transferService);
            userSession = Substitute.For<IUserSession>();

            profilePicture = new ActorProfilePictureModel();
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(profilePicture);
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            userSession = Substitute.For<IUserSession>();
            userSession.SubPageAccess.Returns(new PageAccess());

            nicknameChecker = Substitute.For<NicknameChecker>();
        }

        [TearDown]
        public void Clean()
        {
            sessionManager = null;
            userSession = null;
            defaultHandler = null;
            logoutService = null;
        }

        [Test]
        public void HandleIndexQuery_NotNullQuery_CallStorePasswordHash()
        {
            // Arrange
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, false, false, nicknameChecker);

            // Act
            var viewModel = defaultHandler.Handle(query);

            // Assert
            sessionManager.Received().Add(PasswordHashSessionKey, Arg.Any<string>());
        }

        [Test]
        public async Task HandleIndexQuery_NotNullCommand_ReturnPasswordHashStoredInSession()
        {
            // Arrange
            userSession.IsLogged.Returns(false);
            userSession.IsPasswordExpired.Returns(true);
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, false, false, nicknameChecker);

            string argumentUsed = null;
            sessionManager.Add(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));

            // Act
            var viewModel = await defaultHandler.Handle(query);

            // Assert
            Assert.AreEqual(argumentUsed, viewModel.PasswordHash);
        }

        [Test]
        public async Task HandleIndexQuery_NotNullCommand_ReturnIndexViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "InitialScaleViewPort", "PasswordHash" }
            });
            UrlHelperTestSetUp.SetUp();

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            userSession.RoleId.Returns(RoleId.Agent);
            userSession.IsLogged.Returns(false);
            userSession.IsPasswordExpired.Returns(true);
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, false, false, nicknameChecker);

            string argumentUsed = null;
            sessionManager.Add(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));

            var expectedViewModel = new IndexViewModel(query, getDefaultUrl, encodeDataFunction, browserHelper, profilePicture, false);

            // Act
            var viewModel = await defaultHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedViewModel.IsShowSuggestEnableTelegramOtp, viewModel.IsShowSuggestEnableTelegramOtp);
        }

        #region Handle BrowserQuery

        [Test]
        public void HandleBrowserQuery_BrowserIsUnsupported_ReturnIsUnsupportedBrowserIsTrue()
        {
            //// Arrange
            userSession.RoleId.Returns((byte)4);
            var browserQuery = new BrowserQuery(userSession);
            browserHelper.CheckSupportedBrowsers().Returns(new BrowserViewModel
            {
                IsUnsupportedBrowser = true,
                DownloadNewBrowserLink = "https://www.mozilla.org/firefox"
            });

            //// Action
            var viewModel = defaultHandler.Handle(browserQuery);

            //// Assert
            Assert.IsTrue(viewModel.IsUnsupportedBrowser);
        }

        [Test]
        public void HandleBrowserQuery_UserSawBrowserWarningPageOneTime_SetSawBrowserWarningPageFlagInSession()
        {
            //// Arrange
            userSession.RoleId.Returns((byte)4);
            var browserQuery = new BrowserQuery(userSession);
            browserHelper.CheckSupportedBrowsers().Returns(new BrowserViewModel
            {
                IsUnsupportedBrowser = true,
                DownloadNewBrowserLink = "https://www.mozilla.org/firefox"
            });

            //// Action
            defaultHandler.Handle(browserQuery);

            //// Assert
            this.sessionManager.Received().Add<bool>(SawBrowserWarningPageSessionKey, true);
        }

        [Test]
        public void HandleBrowserQuery_UserSawBrowserWarningPageAlready_ReturnIsUnsupportedBrowserIsFalse()
        {
            //// Arrange
            this.sessionManager.Get<bool>(SawBrowserWarningPageSessionKey).Returns(true);
            var browserQuery = new BrowserQuery(userSession);

            //// Action
            var viewModel = defaultHandler.Handle(browserQuery);

            //// Assert
            Assert.IsFalse(viewModel.IsUnsupportedBrowser);
        }

        #endregion Handle BrowserQuery
    }
}