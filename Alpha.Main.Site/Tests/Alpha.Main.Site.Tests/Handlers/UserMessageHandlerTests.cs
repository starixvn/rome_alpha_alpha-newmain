namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Handlers;
    using Alpha.Main.Site.Models.Requests.UserMessages;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserMessageHandlerTests
    {
        private UserMessageHandler userMessageHandler;

        [SetUp]
        public void SetUp()
        {
            userMessageHandler = new UserMessageHandler();
        }

        [Test]
        public void HandlerHotMessagesQuery_Always_ReturnEmptyList()
        {
            // Arrange
            var message = new HotMessagesQuery();

            // Act
            var act = userMessageHandler.Handle(message);

            // Assert
            Assert.IsEmpty(act);
        }        
    }
}
