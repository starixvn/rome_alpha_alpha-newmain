#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using KellermanSoftware.CompareNetObjects;
    using Main.Models.Notice;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Dashboard.Notice;
    using Site.ViewModels.Dashboard.Notice;
    using Site.ViewModels.Message;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class NoticeHandlerTests
    {
        private NoticeService noticeService;
        private NoticeHandler noticeHandler;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            noticeService = Substitute.For<NoticeService>();
            noticeHandler = new NoticeHandler(noticeService);

            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);
        }

        [Test]
        public void HandleNoticeQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            NoticeQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => noticeHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleNoticeQuery_NotNullQuery_CallGetNoticeService()
        {
            // Arrange
            var noticeMessages = FakeDataHelper.CreateMany<NoticeMessage>().ToList();
            FakeReturnGetNoticeMessagesService(noticeMessages);

            var query = new NoticeQuery(userSession, DateTime.Today, 10, false);

            // Act
            noticeHandler.Handle(query);

            // Assert
            noticeService
                .Received()
                .GetNoticeMessages(
                    Arg.Is<int>(cid => cid == query.CustId),
                    Arg.Is<string>(pid => pid == query.ParentSites),
                    Arg.Is<DateTime>(lpt => lpt == query.LastPostTime),
                    Arg.Is<int>(tm => tm == query.TotalMessage),
                    Arg.Is<bool>(it => it == query.IsTest));
        }

        [Test]
        public void HandleMarkMessageAsReadQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            MarkMessageAsReadQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => noticeHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleMarkMessageAsReadQuery_NotNullQuery_CallMarkMessageAsReadService()
        {
            // Arrange
            var query = new MarkMessageAsReadQuery(CustId.ValidId, 1);

            // Act
            noticeHandler.Handle(query);

            // Assert
            noticeService
                .Received()
                .MarkMessageAsRead(
                    Arg.Is<int>(cid => cid == query.CustId),
                    Arg.Is<int>(mid => mid == query.MessageId));
        }

        [Test]
        public void HandleNoticeQuery_NotNullQuery_ReturnNoticeViewModel()
        {
            // Arrange
            var expectedNoticeMessages = FakeDataHelper.CreateMany<NoticeMessage>().ToList();
            FakeReturnGetNoticeMessagesService(expectedNoticeMessages);

            var query = new NoticeQuery(userSession, DateTime.Today, 100, false);

            // Act
            var viewModel = noticeHandler.Handle(query);

            // Numbers of notice message
            var expectedCount = expectedNoticeMessages
                .Where(m => m.TopLevel >= (byte)MessageLevel.Range.Normal)
                .Count();

            // Assert
            Assert.AreEqual(expectedCount, viewModel.NoticeMessages.Count);
        }

        [Test]
        public void HandleHightlightQuery_NotNullQuery_ReturnNoticeViewModel()
        {
            // Arrange
            var expectedNoticeMessages = FakeDataHelper.CreateMany<NoticeMessage>().ToList();
            FakeReturnGetNoticeMessagesService(expectedNoticeMessages);

            var query = new NoticeQuery(userSession, DateTime.Today, 100, false);

            // Act
            var viewModel = noticeHandler.Handle(query);

            // Numbers of highlight message
            var expectedCount = expectedNoticeMessages.Where(m => m.TopLevel <= (byte)MessageLevel.Range.Highlight).Count();

            // Assert
            Assert.AreEqual(expectedCount, viewModel.HighlightMessages.Count);
        }

        [Test]
        public void Handle_HightlightOrderByTopLevelAsc_ThenByPostTimeDesc_Query_NotNullQuery_ReturnNoticeViewModel()
        {
            // Arrange
            var noticeMessages = new List<NoticeMessage>
            {
                CreateNoticeMessage(1, new DateTime(2017, 5, 1), 1),
                CreateNoticeMessage(2, new DateTime(2017, 5, 2), 3),
                CreateNoticeMessage(3, new DateTime(2017, 5, 2), 1)
            };

            FakeReturnGetNoticeMessagesService(noticeMessages);
            var expectedMessages = new List<NoticeMessageWrapper>
            {
                new NoticeMessageWrapper(CreateNoticeMessage(3, new DateTime(2017, 5, 2), 1), userSession.Language),
                new NoticeMessageWrapper(CreateNoticeMessage(1, new DateTime(2017, 5, 1), 1), userSession.Language),
                new NoticeMessageWrapper(CreateNoticeMessage(2, new DateTime(2017, 5, 2), 3), userSession.Language)
            };

            var query = new NoticeQuery(userSession, DateTime.Today, 100, false);

            var compareLogic = new CompareLogic();

            // Act
            var viewModel = noticeHandler.Handle(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedMessages, viewModel.HighlightMessages).AreEqual);
        }

        [Test]
        public void Handle_NormalOrderByPostTimeDesc_Query_NotNullQuery_ReturnNoticeViewModel()
        {
            // Arrange
            var noticeMessages = new List<NoticeMessage>
            {
                CreateNoticeMessage(1, new DateTime(2017, 5, 1), 41),
                CreateNoticeMessage(2, new DateTime(2017, 5, 2), 33),
                CreateNoticeMessage(3, new DateTime(2017, 5, 2), 45)
            };

            FakeReturnGetNoticeMessagesService(noticeMessages);
            var expectedMessages = new List<NoticeMessageWrapper>
            {
                new NoticeMessageWrapper(CreateNoticeMessage(2, new DateTime(2017, 5, 2), 33), userSession.Language),
                new NoticeMessageWrapper(CreateNoticeMessage(3, new DateTime(2017, 5, 2), 45), userSession.Language),
                new NoticeMessageWrapper(CreateNoticeMessage(1, new DateTime(2017, 5, 1), 41), userSession.Language)
            };

            var query = new NoticeQuery(userSession, DateTime.Today, 100, false);

            var compareLogic = new CompareLogic();

            // Act
            var viewModel = noticeHandler.Handle(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedMessages, viewModel.NoticeMessages).AreEqual);
        }

        private void FakeReturnGetNoticeMessagesService(List<NoticeMessage> noticeMessages)
        {
            noticeService.GetNoticeMessages(
                      Arg.Any<int>(),
                      Arg.Any<string>(),
                      Arg.Any<DateTime>(),
                      Arg.Any<int>(),
                      Arg.Any<bool>())
                  .Returns(noticeMessages);
        }

        private NoticeMessage CreateNoticeMessage(int id, DateTime postTime, byte topLevel)
            => new NoticeMessage(
                id,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                postTime,
                0,
                true,
                postTime,
                string.Empty,
                true,
                topLevel);
    }
}