#pragma warning disable 618, 612

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Helpers;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class DefaultHandlerTests
    {
        private const string PasswordHashSessionKey = "ph";
        private const string SawBrowserWarningPageSessionKey = "DefaultHandler.SawBrowserWarningPageSessionKey";
        private SessionManager sessionManager;
        private DefaultHandler defaultHandler;
        private IUserSession userSession;
        private BrowserHelper browserHelper;

        [SetUp]
        public void SetUp()
        {
            browserHelper = Substitute.For<BrowserHelper>();
            sessionManager = Substitute.For<SessionManager>();
            defaultHandler = new DefaultHandler(sessionManager, browserHelper);
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

            var query = new IndexQuery(userSession, false);

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

            var query = new IndexQuery(userSession, false);

            string argumentUsed = null;
            sessionManager.Add(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));

            // Act
            var viewModel = defaultHandler.Handle(query);

            // Assert
            Assert.AreEqual(argumentUsed, viewModel.PasswordHash);
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
                IsUnsupportedBrowser = true
            });

            //// Action
            var viewModel = defaultHandler.Handle(browserQuery);

            ////
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
                IsUnsupportedBrowser = true
            });

            //// Action
            defaultHandler.Handle(browserQuery);

            ////
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

            ////
            Assert.IsFalse(viewModel.IsUnsupportedBrowser);
        }

        #endregion Handle BrowserQuery
    }
}