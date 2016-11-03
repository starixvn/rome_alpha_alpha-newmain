namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Constants;
    using Fanex.Data.Repository;
    using Main.Constants;
    using Main.Models.Message;
    using Main.Services.Impl;
    using NSubstitute;
    using NSubstitute.ReturnsExtensions;
    using NUnit.Framework;
    using TestData.Helper;
    using Assert = NUnit.Framework.Assert;

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
                1,
                1,
                DateTime.Today,
                DateTime.Today,
                false,
                1);

            // Act & Assert
            Assert.AreEqual(messages, new List<Message>());
        }

        [Test]
        public void GetMessages_CustIdIsValidAndParentSitesIsNotNullorEmpty_ReturnMessages()
        {
            // Arrange
            var messages = FakeDataHelper.CreateMany<Message>();

            dynamicRepository
                .Fetch<Message>(Arg.Any<object>())
                .Returns(messages);

            // Act
            var actualMessages = messageService.GetMessages(
                CustId.ValidId,
                "1",
                1,
                1,
                DateTime.Today,
                DateTime.Today,
                false,
                1);

            // Assert
            Assert.AreEqual(messages, actualMessages);
        }

        [Test]
        public void GetMessagesFromXml_FileDoesNotExist_ReturnEmptyListMessages()
        {
            // Arrange
            var messageXmlFilePath = "//TestData//Messages//Messages.xml";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, MessageType.Public, RoleId.Agent);

            // Assert
            Assert.AreEqual(new List<Message>(), actualMessages);
        }

        [Test]
        public void GetMessagesFromXml_FileExistsAndIsEmpty_CatchXmlExceptionReturnEmptyListMessages()
        {
            // Arrange
            var messageXmlFilePath = $"{GetCurrentProjectDir()}\\TestData\\Messages\\ErrorMessage.xml";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, MessageType.Public, RoleId.Agent);

            // Assert
            Assert.AreEqual(new List<Message>(), actualMessages);
        }

        [TestCase(@"\TestData\Messages\MessageIdEmpty.xml")]
        [TestCase(@"\TestData\Messages\MessageLevelEmpty.xml")]
        [TestCase(@"\TestData\Messages\MessagePostTimeEmpty.xml")]
        [TestCase(@"\TestData\Messages\MessageShowdateEmpty.xml")]
        public void GetMessagesFromXml_FileExistsAndMessageIsNotValid_ExcludeMessage(string messageXmlPath)
        {
            // Arrange
            var messageXmlFilePath = $"{GetCurrentProjectDir()}{messageXmlPath}";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, MessageType.Public, RoleId.Agent);

            // Assert
            Assert.AreEqual(new List<Message>(), actualMessages);
        }

        [Test]
        public void GetMessagesFromXml_FileExistsAndMessageShowDateIsLessThanToDay_ExcludeMessage()
        {
            // Arrange
            var messageXmlFilePath = $"{GetCurrentProjectDir()}\\TestData\\Messages\\MessageShowdateLessThanToday.xml";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, MessageType.Public, RoleId.Agent);

            // Assert
            Assert.AreEqual(new List<Message>(), actualMessages);
        }

        [TestCase(@"\TestData\Messages\MessageLevelInValid.xml", RoleId.Agent)]
        [TestCase(@"\TestData\Messages\MessageLevelAgent.xml", RoleId.Master)]
        [TestCase(@"\TestData\Messages\MessageLevelAgent.xml", RoleId.Super)]
        [TestCase(@"\TestData\Messages\MessageLevelMaster.xml", RoleId.Agent)]
        [TestCase(@"\TestData\Messages\MessageLevelMaster.xml", RoleId.Super)]
        [TestCase(@"\TestData\Messages\MessageLevelSuper.xml", RoleId.Agent)]
        [TestCase(@"\TestData\Messages\MessageLevelSuper.xml", RoleId.Master)]
        public void GetMessagesFromXml_FileExistsAndMessageLevelIsNotValid_ExcludeMessage(string messageXmlPath, byte currentUserLevel)
        {
            // Arrange
            var messageXmlFilePath = $"{GetCurrentProjectDir()}{messageXmlPath}";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, MessageType.Public, currentUserLevel);

            // Assert
            Assert.AreEqual(new List<Message>(), actualMessages);
        }

        [TestCase(@"\TestData\Messages\Message.xml", RoleId.Master)]
        [TestCase(@"\TestData\Messages\MessageLevelAgent.xml", RoleId.Agent)]
        [TestCase(@"\TestData\Messages\MessageLevelMaster.xml", RoleId.Master)]
        [TestCase(@"\TestData\Messages\MessageLevelSuper.xml", RoleId.Super)]
        public void GetMessagesFromXml_FileExistsAndMessageIsValid_IncludeMessage(string messageXmlPath, byte currentUserLevel)
        {
            // Arrange
            var messageXmlFilePath = $"{GetCurrentProjectDir()}{messageXmlPath}";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, MessageType.Public, currentUserLevel);

            // Assert
            Assert.AreEqual(1, actualMessages.Count);
        }

        [TestCase(MessageType.Public)]
        [TestCase(MessageType.Special)]
        [TestCase(MessageType.Private)]
        public void GetMessagesFromXml_FileExistsAndMessageIsValid_ExactMessageContent(int messageType)
        {
            // Arrange
            var messageXmlFilePath = $"{GetCurrentProjectDir()}\\TestData\\Messages\\Message.xml";

            // Act
            var actualMessages = messageService.GetMessagesFromXml(messageXmlFilePath, messageType, RoleId.Master);

            // Assert
            var message = GetExpectedMessageObject(messageType);
            var actualMessage = actualMessages.FirstOrDefault();
            Assert.IsTrue(message.ContentEN == actualMessage.ContentEN &&
                message.ContentCH == actualMessage.ContentCH &&
                message.ContentCS == actualMessage.ContentCS &&
                message.ContentJP == actualMessage.ContentJP &&
                message.ContentKO == actualMessage.ContentKO &&
                message.ContentTH == actualMessage.ContentTH &&
                message.ContentVN == actualMessage.ContentVN &&
                message.MessageType == actualMessage.MessageType &&
                message.PostTime == actualMessage.PostTime);
        }

        private Message GetExpectedMessageObject(int messageType)
        {
            var message = new Message(
                        88888890,
                        "EN",
                        "CN",
                        "TW",
                        "JP",
                        "TH",
                        string.Empty,
                        "KR",
                        string.Empty,
                        "VN",
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        new DateTime(2016, 1, 20, 0, 0, 0),
                        new DateTime(2016, 1, 20, 0, 0, 0),
                        new DateTime(2016, 1, 20, 0, 0, 0),
                        (byte)messageType,
                        false,
                        false,
                        0,
                        false,
                        0,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        0);

            return message;
        }

        private string GetCurrentProjectDir()
            => Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
    }
}