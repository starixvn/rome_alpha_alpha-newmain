namespace Alpha.Main.Site.Tests.Requests.Message
{
    using System;
    using Main.Constants;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Requests.Message;
    using Sunplus.Agent.Model.Entities;
    using Language = Constants.Language;
    using RoleId = Constants.RoleId;

    [TestFixture]
    public class MessageQueryTests
    {
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);
        }

        [Test]
        public void CustId_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var custId = 69;
            userSession.CustId.Returns(custId);

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                DateTime.Today,
                DateTime.Today,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(custId, query.CustId);
        }

        [TestCase(RoleId.Super, "3000,3100")]
        [TestCase(RoleId.Master, "3000,3200")]
        [TestCase(RoleId.Agent, "3000,3300")]
        public void ParentSites_Always_ReturnCorrectByUserLevelId(byte userLevelId, string expectedParentSitesId)
        {
            // Arrange
            userSession.RoleId.Returns(userLevelId);

            // Act
            var query = new MessageQuery(
                    userSession,
                    MessageType.Public,
                    DateTime.Today,
                    DateTime.Today,
                    false,
                    string.Empty);

            // Assert
            Assert.AreEqual(expectedParentSitesId, query.ParentSites);
        }

        [Test]
        public void MessageType_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var msgType = MessageType.Public;

            // Act
            var query = new MessageQuery(
                userSession,
                msgType,
                DateTime.Today,
                DateTime.Today,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(msgType, query.MessageType);
        }

        [TestCase(MessageType.Public, 0)]
        [TestCase(MessageType.Special, 1)]
        [TestCase(MessageType.Private, 2)]
        public void SpecialType_Always_ReturnValueByMessageType(int messageType, int specialType)
        {
            // Act
            var query = new MessageQuery(
                userSession,
                messageType,
                DateTime.Today,
                DateTime.Today,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(specialType, query.SpecialType);
        }

        [TestCase(MessageType.Public)]
        [TestCase(MessageType.Private)]
        public void StartDate_WhenMessageTypeIsNotSpecial_EqualsValueSetFromCtor(int messageType)
        {
            // Arrange
            var startDate = DateTime.Today;

            // Act
            var query = new MessageQuery(
                userSession,
                messageType,
                startDate,
                DateTime.Today,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(startDate, query.StartDate);
        }

        [Test]
        public void StartDate_WhenMessageTypeIsSpecial_ReturnMinDateTimeValue()
        {
            // Arrange
            var startDate = new DateTime(1900, 1, 1);

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Special,
                startDate,
                DateTime.Today,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(startDate, query.StartDate);
        }

        [Test]
        public void EndDate_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var endDate = DateTime.Today;

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                DateTime.Today,
                endDate,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(endDate, query.EndDate);
        }

        [Test]
        public void IsTest_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var isTest = false;

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                DateTime.Today,
                DateTime.Today,
                isTest,
                string.Empty);

            // Assert
            Assert.AreEqual(isTest, query.IsTest);
        }

        [Test]
        public void CurrentLang_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var currentLang = Language.English;
            userSession.Language.Returns(currentLang);

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                DateTime.Today,
                DateTime.Today,
                false,
                string.Empty);

            // Assert
            Assert.AreEqual(currentLang, query.CurrentLang);
        }

        [Test]
        public void TotalMessage_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            var totalMessage = 69;

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                DateTime.Today,
                DateTime.Today,
                false,
                string.Empty,
                totalMessage);

            // Assert
            Assert.AreEqual(totalMessage, query.TotalMessage);
        }

        [Test]
        public void RoleId_Always_EqualsValueSetFromCtor()
        {
            // Arrange
            userSession.RoleId.Returns(RoleId.Agent);

            // Act
            var query = new MessageQuery(
                userSession,
                MessageType.Public,
                DateTime.Today,
                DateTime.Today,
                false,
                string.Empty,
                50);

            // Assert
            Assert.AreEqual(RoleId.Agent, query.RoleId);
        }

        [TestCase(MessageType.Public, "NormalMessage.xml")]
        [TestCase(MessageType.Special, "SpecialMessage.xml")]
        [TestCase(MessageType.Private, "PrivateMessage.xml")]
        public void MessageXmlFilePath_Always_GetByMessageType(int messageType, string messageXmlFileName)
        {
            // Arrange
            var messageXmlDirPath = "dirpath";

            // Act
            var query = new MessageQuery(
                userSession,
                messageType,
                DateTime.Today,
                DateTime.Today,
                false,
                messageXmlDirPath,
                50);

            // Assert
            Assert.AreEqual($"{messageXmlDirPath}/{messageXmlFileName}", query.MessageXmlFilePath);
        }
    }
}