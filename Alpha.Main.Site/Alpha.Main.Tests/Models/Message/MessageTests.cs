namespace Alpha.Main.Tests.Models.Message
{
    using System;
    using System.Collections.Generic;
    using Main.Models.Message;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class MessageTests
    {
        [Test]
        public void Id_Always_SetValueFromCtor()
        {
            // Arrange
            int id = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("id", id);

            // Assert
            Assert.AreEqual(id, message.Id);
        }

        [Test]
        public void ContentEN_Always_SetValueFromCtor()
        {
            // Arrange
            string contentEN = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentEN", contentEN);

            // Assert
            Assert.AreEqual(contentEN, message.ContentEN);
        }

        [Test]
        public void ContentCS_Always_SetValueFromCtor()
        {
            // Arrange
            string contentCS = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentCS", contentCS);

            // Assert
            Assert.AreEqual(contentCS, message.ContentCS);
        }

        [Test]
        public void ContentCH_Always_SetValueFromCtor()
        {
            // Arrange
            string contentCH = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentCH", contentCH);

            // Assert
            Assert.AreEqual(contentCH, message.ContentCH);
        }

        [Test]
        public void ContentJP_Always_SetValueFromCtor()
        {
            // Arrange
            string contentJP = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentJP", contentJP);

            // Assert
            Assert.AreEqual(contentJP, message.ContentJP);
        }

        [Test]
        public void ContentTH_Always_SetValueFromCtor()
        {
            // Arrange
            string contentTH = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentTH", contentTH);

            // Assert
            Assert.AreEqual(contentTH, message.ContentTH);
        }

        [Test]
        public void ContentIT_Always_SetValueFromCtor()
        {
            // Arrange
            string contentIT = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentIT", contentIT);

            // Assert
            Assert.AreEqual(contentIT, message.ContentIT);
        }

        [Test]
        public void ContentKO_Always_SetValueFromCtor()
        {
            // Arrange
            string contentKO = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentKO", contentKO);

            // Assert
            Assert.AreEqual(contentKO, message.ContentKO);
        }

        [Test]
        public void ContentES_Always_SetValueFromCtor()
        {
            // Arrange
            string contentES = "value";

            // Act
            var message = FakeDataHelper.Create<Message>("contentES", contentES);

            // Assert
            Assert.AreEqual(contentES, message.ContentES);
        }

        [Test]
        public void PostTime_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime postTime = DateTime.Today;

            // Act
            var message = FakeDataHelper.Create<Message>("postTime", postTime);

            // Assert
            Assert.AreEqual(postTime, message.PostTime);
        }

        [Test]
        public void CreateTime_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime createTime = DateTime.Today;

            // Act
            var message = FakeDataHelper.Create<Message>("createTime", createTime);

            // Assert
            Assert.AreEqual(createTime, message.CreateTime);
        }

        [Test]
        public void LastUpdateTime_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime lastUpdateTime = DateTime.Today;

            // Act
            var message = FakeDataHelper.Create<Message>("lastUpdateTime", lastUpdateTime);

            // Assert
            Assert.AreEqual(lastUpdateTime, message.LastUpdateTime);
        }

        [Test]
        public void MessageType_Always_SetValueFromCtor()
        {
            // Arrange
            byte messageType = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("messageType", messageType);

            // Assert
            Assert.AreEqual(messageType, message.MessageType);
        }

        [Test]
        public void IsSticky_Always_SetValueFromCtor()
        {
            // Arrange
            bool isSticky = false;

            // Act
            var message = FakeDataHelper.Create<Message>("isSticky", isSticky);

            // Assert
            Assert.AreEqual(isSticky, message.IsSticky);
        }

        [Test]
        public void IsTest_Always_SetValueFromCtor()
        {
            // Arrange
            bool isTest = false;

            // Act
            var message = FakeDataHelper.Create<Message>("isTest", isTest);

            // Assert
            Assert.AreEqual(isTest, message.IsTest);
        }

        [Test]
        public void MatchId_Always_SetValueFromCtor()
        {
            // Arrange
            int matchId = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("matchId", matchId);

            // Assert
            Assert.AreEqual(matchId, message.MatchId);
        }

        [Test]
        public void IsBatch_Always_SetValueFromCtor()
        {
            // Arrange
            bool isBatch = false;

            // Act
            var message = FakeDataHelper.Create<Message>("isBatch", isBatch);

            // Assert
            Assert.AreEqual(isBatch, message.IsBatch);
        }

        [Test]
        public void CreatorId_Always_SetValueFromCtor()
        {
            // Arrange
            int creatorId = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("creator", creatorId);

            // Assert
            Assert.AreEqual(creatorId, message.CreatorId);
        }

        [Test]
        public void LastUpdatorId_Always_SetValueFromCtor()
        {
            // Arrange
            int lastUpdator = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("lastUpdator", lastUpdator);

            // Assert
            Assert.AreEqual(lastUpdator, message.LastUpdator);
        }

        [Test]
        public void UserId_Always_SetValueFromCtor()
        {
            // Arrange
            int userId = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("userId", userId);

            // Assert
            Assert.AreEqual(userId, message.UserId);
        }

        [Test]
        public void CreatorName_Always_SetValueFromCtor()
        {
            // Arrange
            string creatorName = "user";

            // Act
            var message = FakeDataHelper.Create<Message>("creatorName", creatorName);

            // Assert
            Assert.AreEqual(creatorName, message.CreatorName);
        }

        [Test]
        public void LastUpdatorName_Always_SetValueFromCtor()
        {
            // Arrange
            string lastUpdateName = "user";

            // Act
            var message = FakeDataHelper.Create<Message>("lastUpdateName", lastUpdateName);

            // Assert
            Assert.AreEqual(lastUpdateName, message.LastUpdateName);
        }

        [Test]
        public void UserName_Always_SetValueFromCtor()
        {
            // Arrange
            string userName = "user";

            // Act
            var message = FakeDataHelper.Create<Message>("userName", userName);

            // Assert
            Assert.AreEqual(userName, message.UserName);
        }

        [Test]
        public void BettransId_Always_SetValueFromCtor()
        {
            // Arrange
            string bettransId = "1";

            // Act
            var message = FakeDataHelper.Create<Message>("bettransId", bettransId);

            // Assert
            Assert.AreEqual(bettransId, message.BetTransId);
        }

        [Test]
        public void RowNum_Always_SetValueFromCtor()
        {
            // Arrange
            long rowNum = 1;

            // Act
            var message = FakeDataHelper.Create<Message>("rowNum", rowNum);

            // Assert
            Assert.AreEqual(rowNum, message.RowNum);
        }        
    }
}