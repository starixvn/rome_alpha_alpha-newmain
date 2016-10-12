namespace Alpha.Main.Site.Tests.ViewModels.Message
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Constants;
    using Main.Models.Message;
    using NUnit.Framework;
    using Site.ViewModels.Message;

    public class MessagesViewModelTests
    {
        [Test]
        public void StartDate_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var messageWrappers = new List<MessageWrapper>();
            messageWrappers.Add(CreateMessageWrapperDataWithPostTime(DateTime.Today));
            var startDate = DateTime.Today;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                startDate,
                DateTime.Today,
                MessageType.Public);

            // Assert
            Assert.AreEqual(startDate, viewModel.StartDate);
        }

        [Test]
        public void EndDate_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var messageWrappers = new List<MessageWrapper>();
            messageWrappers.Add(CreateMessageWrapperDataWithPostTime(DateTime.Today));
            var endDate = DateTime.Today;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                endDate,
                MessageType.Public);

            // Assert
            Assert.AreEqual(endDate, viewModel.EndDate);
        }

        [Test]
        public void MessageType_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var messageWrappers = new List<MessageWrapper>();
            messageWrappers.Add(CreateMessageWrapperDataWithPostTime(DateTime.Today));
            var msgType = MessageType.Public;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                msgType);

            // Assert
            Assert.AreEqual(msgType, viewModel.MessageType);
        }

        [TestCase(MessageType.Public, "public")]
        [TestCase(MessageType.Special, "special")]
        [TestCase(MessageType.Private, "private")]
        public void MessageTypeName_Always_ReturnValueByMessageType(int messageType, string messageTypeName)
        {
            // Arrange
            var messageWrappers = new List<MessageWrapper>();
            messageWrappers.Add(CreateMessageWrapperDataWithPostTime(DateTime.Today));

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                messageType);

            // Assert
            Assert.AreEqual(messageTypeName, viewModel.MessageTypeName);
        }

        [Test]
        public void Messages_Always_OrderByDescendingPostTime()
        {
            // Arrange
            var messageWrappers = new List<MessageWrapper>();
            messageWrappers.Add(CreateMessageWrapperDataWithPostTime(DateTime.Today));
            messageWrappers.Add(CreateMessageWrapperDataWithPostTime(DateTime.Today.AddDays(1)));

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                MessageType.Public);

            // Assert
            Assert.AreEqual(
                messageWrappers.Last().CurrentMessage.PostTime,
                viewModel.Messages.First().CurrentMessage.PostTime);
        }

        private MessageWrapper CreateMessageWrapperDataWithPostTime(DateTime? postTime)
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
                postTime ?? DateTime.Today,
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

            return new MessageWrapper(message, Language.English);
        }
    }
}