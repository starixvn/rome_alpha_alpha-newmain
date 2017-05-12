namespace Alpha.Main.Tests.Models.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using Main.Models.Notice;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class NoticeMessageTests
    {
        [Test]
        public void Id_Always_SetValueFromCtor()
        {
            // Arrange
            int id = 100;

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("id", id);

            // Assert
            Assert.AreEqual(id, noticeMessage.Id);
        }

        [Test]
        public void ContentEN_Always_SetValueFromCtor()
        {
            // Arrange
            string contentEN = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentEN", contentEN);

            // Assert
            Assert.AreEqual(contentEN, noticeMessage.ContentEN);
        }

        [Test]
        public void ContentCS_Always_SetValueFromCtor()
        {
            // Arrange
            string contentCS = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentCS", contentCS);

            // Assert
            Assert.AreEqual(contentCS, noticeMessage.ContentCS);
        }

        [Test]
        public void ContentCH_Always_SetValueFromCtor()
        {
            // Arrange
            string contentCH = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentCH", contentCH);

            // Assert
            Assert.AreEqual(contentCH, noticeMessage.ContentCH);
        }

        [Test]
        public void ContentJP_Always_SetValueFromCtor()
        {
            // Arrange
            string contentJP = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentJP", contentJP);

            // Assert
            Assert.AreEqual(contentJP, noticeMessage.ContentJP);
        }

        [Test]
        public void ContentTH_Always_SetValueFromCtor()
        {
            // Arrange
            string contentTH = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentTH", contentTH);

            // Assert
            Assert.AreEqual(contentTH, noticeMessage.ContentTH);
        }

        [Test]
        public void ContentIT_Always_SetValueFromCtor()
        {
            // Arrange
            string contentIT = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentIT", contentIT);

            // Assert
            Assert.AreEqual(contentIT, noticeMessage.ContentIT);
        }

        [Test]
        public void ContentKO_Always_SetValueFromCtor()
        {
            // Arrange
            string contentKO = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentKO", contentKO);

            // Assert
            Assert.AreEqual(contentKO, noticeMessage.ContentKO);
        }

        [Test]
        public void ContentES_Always_SetValueFromCtor()
        {
            // Arrange
            string contentES = "value";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("contentES", contentES);

            // Assert
            Assert.AreEqual(contentES, noticeMessage.ContentES);
        }

        [Test]
        public void PostTime_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime postTime = DateTime.Today;

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("postTime", postTime);

            // Assert
            Assert.AreEqual(postTime, noticeMessage.PostTime);
        }

        [Test]
        public void LastUpdateTime_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime lastUpdateTime = DateTime.Today;

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("lastUpdateTime", lastUpdateTime);

            // Assert
            Assert.AreEqual(lastUpdateTime, noticeMessage.LastUpdateTime);
        }

        [Test]
        public void Type_Always_SetValueFromCtor()
        {
            // Arrange
            byte messageType = 1;

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("type", messageType);

            // Assert
            Assert.AreEqual(messageType, noticeMessage.MessageType);
        }

        [Test]
        public void IsSticky_Always_SetValueFromCtor()
        {
            // Arrange
            bool isSticky = false;

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("isSticky", isSticky);

            // Assert
            Assert.AreEqual(isSticky, noticeMessage.IsSticky);
        }

        [Test]
        public void BetTransId_Always_SetValueFromCtor()
        {
            // Arrange
            string betTransId = "1";

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("betTransId", betTransId);

            // Assert
            Assert.AreEqual(betTransId, noticeMessage.BetTransId);
        }

        [Test]
        public void IsRead_Always_SetValueFromCtor()
        {
            // Arrange
            bool isRead = false;

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("isRead", isRead);

            // Assert
            Assert.AreEqual(isRead, noticeMessage.IsRead);
        }

        [Test]
        public void IsTopLevel_Always_SetValueFromCtor()
        {
            // Arrange
            List<int> range = new List<int>();
            for (int i = 1; i <= 255; i++)
            {
                range.Add(i);
            }

            // Act
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("topLevel", range);

            // Assert
            Assert.Contains(noticeMessage.TopLevel, range);
        }
    }
}