using Alpha.Main.Services;
using Alpha.Main.Site.Handlers;
using Alpha.Main.Site.Models.Requests.UserMessages;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Alpha.Main.Site.Tests.Handlers
{
    [TestFixture]
    public class UserMessageHandlerTests
    {
        private UserMessageHandler userMessageHandler;
        private UserMessageService userMessageService;

        [SetUp]
        public void SetUp()
        {
            userMessageService = Substitute.For<UserMessageService>();
            userMessageHandler = new UserMessageHandler(userMessageService);
        }

        [Test]
        public void HandlerHotMessagesQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            HotMessagesQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => userMessageHandler.Handle(message));
        }

        [Test]
        public void HandlerHotMessagesQuery_MessageIsNotNull_CallsGetHotMessages()
        {
            // Arrange
            var message = new HotMessagesQuery("HotMessageFilePath");

            // Act
            userMessageHandler.Handle(message);

            // Assert
            userMessageService.Received().GetHotMessages(
                Arg.Is<string>(path => path == message.HotMessageFilePath),
                Arg.Is<byte>(roleId => roleId == message.RoleId),
                Arg.Is<string>(lang => lang == message.Language));
        }

        [Test]
        public void HandlerHotMessagesQuery_MessageIsNotNull_ReturnsHotMessages()
        {
            // Arrange
            var expectedHotMessages = new List<string>();
            userMessageService.GetHotMessages(Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>()).Returns(expectedHotMessages);
            var message = new HotMessagesQuery("HotMessageFilePath");

            // Act
            var hotMessages = userMessageHandler.Handle(message);

            // Assert
            Assert.AreEqual(expectedHotMessages, hotMessages);
        }
    }
}
