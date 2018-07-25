namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using SecurityCode.Web;
    using Site.Handlers;
    using Site.Helpers;
    using Site.Requests;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

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

        [SetUp]
        public void SetUp()
        {
            browserHelper = Substitute.For<BrowserHelper>();
            sessionManager = Substitute.For<SessionManager>();
            getDefaultUrl = Substitute.For<Func<string>>();
            encodeDataFunction = Substitute.For<Func<string, string>>();
            defaultHandler = new DefaultHandler(sessionManager, browserHelper, getDefaultUrl, encodeDataFunction);
            userSession = Substitute.For<IUserSession>();
        }

        [TearDown]
        public void Clean()
        {
            sessionManager = null;
            userSession = null;
            defaultHandler = null;
        }

        [Test]
        public void HandleIndexQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            IndexQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => defaultHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleIndexQuery_NotNullQuery_CallStorePasswordHash()
        {
            // Arrange
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, false, false);

            // Act
            var viewModel = defaultHandler.Handle(query);

            // Assert
            sessionManager.Received().Add(PasswordHashSessionKey, Arg.Any<string>());
        }

        [Test]
        public void HandleIndexQuery_NotNullCommand_ReturnPasswordHashStoredInSession()
        {
            // Arrange
            userSession.IsLogged.Returns(false);
            userSession.IsPasswordExpired.Returns(true);
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, false, false);

            string argumentUsed = null;
            sessionManager.Add(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));

            // Act
            var viewModel = defaultHandler.Handle(query);

            // Assert
            Assert.AreEqual(argumentUsed, viewModel.PasswordHash);
        }

        [Test]
        public void HandleIndexQuery_NotNullCommand_ReturnIndexViewModel()
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

            var query = new IndexQuery(userSession, false, false);

            string argumentUsed = null;
            sessionManager.Add(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));

            var expectedViewModel = new IndexViewModel(query, getDefaultUrl, encodeDataFunction, browserHelper);

            // Act
            var viewModel = defaultHandler.Handle(query);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel, viewModel).AreEqual);
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