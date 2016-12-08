namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Constants;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Main.Constants;
    using Main.Models.Message;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using TestData.Helper;
    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public class MessageServiceImplTests
    {
        private MessageDataAccess messageDataAccess;
        private MessageServiceImpl messageService;

        [SetUp]
        public void SetUp()
        {
            messageDataAccess = Substitute.For<MessageDataAccess>();

            messageService = new MessageServiceImpl(messageDataAccess);
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
            Assert.AreEqual(messages, null);
        }

        [Test]
        public void GetMessages_CustIdIsValidAndParentSitesIsNotNullorEmpty_ReturnMessages()
        {
            // Arrange
            var messages = FakeDataHelper.CreateMany<Message>().ToList();

            messageDataAccess.GetMessages(Arg.Any<ICriteria>(), Arg.Any<int>()).Returns(messages);

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
                        id: 88888890,
                        contentEN: "EN",
                        contentCS: "CN",
                        contentCH: "TW",
                        contentJP: "JP",
                        contentTH: "TH",
                        contentIT: string.Empty,
                        contentKO: "KR",
                        contentES: string.Empty,
                        contentVN: "VN",
                        postTime: new DateTime(2016, 1, 20, 0, 0, 0),
                        createTime: new DateTime(2016, 1, 20, 0, 0, 0),
                        lastUpdateTime: new DateTime(2016, 1, 20, 0, 0, 0),
                        messageType: (byte)messageType,
                        isSticky: false,
                        isTest: false,
                        matchId: 0,
                        isBatch: false,
                        creator: 0,
                        lastUpdator: 0,
                        creatorName: string.Empty,
                        lastUpdateName: string.Empty,
                        userName: string.Empty,
                        bettransId: string.Empty,
                        userId: 0,
                        rowNum: 0);

            return message;
        }

        private string GetCurrentProjectDir()
            => Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
    }
}