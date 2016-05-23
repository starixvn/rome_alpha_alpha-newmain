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

        [Test]
        public void HandleIndexQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            IndexQuery query = null;

            var sessionManager = Substitute.For<SessionManager>();
            var defaultHandler = new DefaultHandler(sessionManager);

            // Action & Assert
            Assert.Throws<NullReferenceException>(new TestDelegate(() => defaultHandler.Handle(query)));
        }

        [Test]
        public void HandleIndexQuery_NotNullCommand_CallStorePasswordHash()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.SubPageAccess.Returns(new PageAccess());

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            var query = new IndexQuery(userSession, securityCodeSession, false);

            var sessionManager = Substitute.For<SessionManager>();

            var defaultHandler = new DefaultHandler(sessionManager);

            // Action 
            var viewModel = defaultHandler.Handle(query);

            // Assert 
            sessionManager.Received().Add<string>(PasswordHashSessionKey, Arg.Any<string>());
        }

        [Test]
        public void HandleIndexQuery_NotNullCommand_ReturnPasswordHashStoredInSession()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.SubPageAccess.Returns(new PageAccess());

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            var query = new IndexQuery(userSession, securityCodeSession, false);

            var sessionManager = Substitute.For<SessionManager>();
            string argumentUsed = null;
            sessionManager.Add<string>(PasswordHashSessionKey, Arg.Do<string>(x => argumentUsed = x));
            var defaultHandler = new DefaultHandler(sessionManager);

            // Action 
            var viewModel = defaultHandler.Handle(query);

            // Assert 
            Assert.AreEqual(argumentUsed, viewModel.PasswordHash);
        }
    }
}
