namespace Alpha.Main.Site.Tests.ViewModels.Message
{
    using System;
    using System.Linq;
    using Main.Constants;
    using Main.Tests.TestData.Helper;
    using NUnit.Framework;
    using Site.ViewModels.Message;
    using RoleId = Constants.RoleId;

    public class MessagesViewModelTests
    {
        [Test]
        public void StartDate_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var messageWrappers = FakeDataHelper.CreateMany<MessageWrapper>().ToList();
            var startDate = DateTime.Today;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                startDate,
                DateTime.Today,
                MessageType.Public,
                RoleId.Agent);

            // Assert
            Assert.AreEqual(startDate, viewModel.StartDate);
        }

        [Test]
        public void EndDate_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var messageWrappers = FakeDataHelper.CreateMany<MessageWrapper>().ToList();
            var endDate = DateTime.Today;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                endDate,
                MessageType.Public,
                RoleId.Agent);

            // Assert
            Assert.AreEqual(endDate, viewModel.EndDate);
        }

        [Test]
        public void MessageType_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var messageWrappers = FakeDataHelper.CreateMany<MessageWrapper>().ToList();
            var msgType = MessageType.Public;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                msgType,
                RoleId.Agent);

            // Assert
            Assert.AreEqual(msgType, viewModel.MessageType);
        }

        [TestCase(MessageType.Public, "public")]
        [TestCase(MessageType.Special, "special")]
        [TestCase(MessageType.Private, "private")]
        public void MessageTypeName_Always_ReturnValueByMessageType(int messageType, string messageTypeName)
        {
            // Arrange
            var messageWrappers = FakeDataHelper.CreateMany<MessageWrapper>().ToList();

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                messageType,
                RoleId.Agent);

            // Assert
            Assert.AreEqual(messageTypeName, viewModel.MessageTypeName);
        }

        [TestCase(RoleId.Super, "super")]
        [TestCase(RoleId.Master, "master")]
        [TestCase(RoleId.Agent, "agent")]
        public void UserLevelName_Always_ReturnValueByUserLevel(byte roleId, string userLevelName)
        {
            // Arrange
            var messageWrappers = FakeDataHelper.CreateMany<MessageWrapper>().ToList();

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                MessageType.Public,
                roleId);

            // Assert
            Assert.AreEqual(userLevelName, viewModel.UserLevelName);
        }

        [Test]
        public void Messages_Always_OrderByDescendingPostTime()
        {
            // Arrange
            var messageWrappers = FakeDataHelper.CreateMany<MessageWrapper>().ToList();
            var firstPostTime = messageWrappers.OrderByDescending(mw => mw.CurrentMessage.PostTime).First().CurrentMessage.PostTime;

            // Act
            var viewModel = new MessagesViewModel(
                messageWrappers,
                DateTime.Today,
                DateTime.Today,
                MessageType.Public,
                RoleId.Agent);

            // Assert
            Assert.AreEqual(
                firstPostTime,
                viewModel.Messages.First().CurrentMessage.PostTime);
        }
    }
}