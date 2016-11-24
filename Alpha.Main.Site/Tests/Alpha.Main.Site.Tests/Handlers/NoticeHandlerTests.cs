#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Main.Models.Notice;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Dashboard.Notice;
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

            // Assert
            Assert.AreEqual(expectedNoticeMessages.Count, viewModel.NoticeMessages.Count);
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
    }
}