namespace Alpha.Main.Site.Tests.Handlers
{
    using Fanex.Razorbill.Security.Sessions;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Sunplus.Agent.Model.Entities;
    using System;

    [TestFixture]
    public class DefaultHandlerTests
    {
        private const string PasswordHashSessionKey = "ph";
        private SessionManager sessionManager;
        private DefaultHandler defaultHandler;
        private IUserSession userSession;
        private ISecurityCodeSession securityCodeSession;

        [SetUp]
        public void SetUp()
        {
            sessionManager = Substitute.For<SessionManager>();
            defaultHandler = new DefaultHandler(sessionManager);
            userSession = Substitute.For<IUserSession>();
            securityCodeSession = Substitute.For<ISecurityCodeSession>();
        }

        [Test]
        public void HandleIndexQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            IndexQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => defaultHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleIndexQuery_NotNullQuery_CallStorePasswordHash()
        {
            // Arrange
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, securityCodeSession, false);

            // Action 
            var viewModel = defaultHandler.Handle(query);

            // Assert 
            sessionManager.Received().Add<string>(PasswordHashSessionKey, Arg.Any<string>());
        }

        [Test]
        public void HandleIndexQuery_NotNullCommand_ReturnPasswordHashStoredInSession()
        {
            // Arrange
            userSession.SubPageAccess.Returns(new PageAccess());

            var query = new IndexQuery(userSession, securityCodeSession, false);

            string argumentUsed = null;
            sessionManager.Add<string>(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));

            // Action 
            var viewModel = defaultHandler.Handle(query);

            // Assert 
            Assert.AreEqual(argumentUsed, viewModel.PasswordHash);
        }
    }
}
