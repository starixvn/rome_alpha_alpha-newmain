namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Data.Repository;
    using Main.Models.Message;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MessageServiceImplTests
    {
        private IDynamicRepository dynamicRepository;

        private MessageServiceImpl messageService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            messageService = new MessageServiceImpl(dynamicRepository);
        }

        [Test]
        public void GetMessages_CustIdIsInvalidAndParentSitesIsNullOrEmpty_ReturnEmpty()
        {
            // Arrange
            var messages = messageService.GetMessages(
                CustId.InvalidId,
                string.Empty,
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<bool>(),
                Arg.Any<int>());

            // Act & Assert
            Assert.AreEqual(messages, new List<Message>());
        }

        [Test]
        public void GetMessages_CustIdIsValidAndParentSitesIsNotNullorEmpty_ReturnMessages()
        {
            // Arrange
            var messages = new List<Message>();
            messages.Add(CreateMessageData());

            dynamicRepository
                .Fetch<Message>(Arg.Any<object>())
                .Returns(messages);

            // Act
            var actualMessages = messageService.GetMessages(
                CustId.ValidId,
                "1",
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<bool>(),
                Arg.Any<int>());

            // Assert
            Assert.AreEqual(messages, actualMessages);
        }

        private Message CreateMessageData()
        {
            var message = new Message(
                1,
                "en",
                "cs",
                "ch",
                "jp",
                "th",
                "it",
                "ko",
                "es",
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                1,
                false,
                false,
                1,
                true,
                1,
                1,
                "name",
                "name",
                false,
                "1",
                1,
                "name",
                1);

            return message;
        }
    }
}