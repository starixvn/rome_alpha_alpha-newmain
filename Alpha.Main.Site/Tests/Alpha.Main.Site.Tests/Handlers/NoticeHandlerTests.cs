namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Main.Models.Notice;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Notice;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class NoticeHandlerTests
    {
        private NoticeService noticeService;
        private NoticeHandler noticeHandler;
        private IUserSession userSession;

        private NoticeMessage noticeMessage;

        public object DataTime { get; private set; }

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

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => noticeHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleNoticeQuery_NotNullQuery_CallGetBalanceInfoService()
        {
            // Arrange
            var query = new NoticeQuery(userSession, DateTime.Today, 10, false);

            // Action
            var viewModel = noticeHandler.Handle(query);

            // Assert
            noticeService
                .Received()
                .GetNoticeMessages(
                    Arg.Is<int>(cid => cid == query.CustId),
                    Arg.Is<string>(pid => pid == query.ParentSiteId),
                    Arg.Is<DateTime>(lpt => lpt == query.LastPostTime),
                    Arg.Is<int>(tm => tm == query.TotalMessage),
                    Arg.Is<bool>(it => it == query.IsTest));
        }

        [TestCase(1, false, "private")]
        [TestCase(1, true, "special")]
        [TestCase(3, true, "special")]
        [TestCase(3, false, "public")]
        public void HandleNoticeQuery_NotNullQuery_GetNoticeMessageType(
            byte msgType,
            bool isSticky,
            string expectedMsgTypeText)
        {
            // Arrange
            var noticeMessages = CreateNoticeMessagesData(msgType, isSticky);
            FakeReturnGetNoticeMessagesService(noticeMessages);

            var noticeQuery = new NoticeQuery(userSession, DateTime.Today, 10, false);

            // Action
            var viewModel = noticeHandler.Handle(noticeQuery);

            // Assert
            Assert.AreEqual(expectedMsgTypeText, viewModel.NoticeMessages.FirstOrDefault().Type);
        }

        [TestCase(Language.ChineseSimplified, "cs")]
        [TestCase(Language.ChineseTraditional, "ch")]
        [TestCase(Language.Thai, "th")]
        [TestCase(Language.Japanese, "jp")]
        [TestCase(Language.Korean, "ko")]
        [TestCase(Language.Vietnamese, "en")]
        [TestCase(Language.English, "en")]
        public void HandleNoticeQuery_NotNullQuery_GetNoticeContentByLang(string lang, string expectedContent)
        {
            // Arrange
            var noticeMessages = CreateNoticeMessagesData();
            FakeReturnGetNoticeMessagesService(noticeMessages);

            userSession.Language.Returns(lang);

            var noticeQuery = new NoticeQuery(userSession, DateTime.Today, 10, false);

            // Action
            var viewModel = noticeHandler.Handle(noticeQuery);

            // Assert
            Assert.AreEqual(expectedContent, viewModel.NoticeMessages.FirstOrDefault().Content);
        }

        [Test]
        public void HandleNoticeQuery_NotNullQuery_GetNoticeShortContent()
        {
            // Arrange
            var noticeMessages = CreateNoticeMessagesData();
            FakeReturnGetNoticeMessagesService(noticeMessages);

            userSession.Language.Returns(Language.English);

            var noticeQuery = new NoticeQuery(userSession, DateTime.Today, 10, false);

            // Action
            var viewModel = noticeHandler.Handle(noticeQuery);

            // Assert
            Assert.AreEqual("en...", viewModel.NoticeMessages.FirstOrDefault().ShortContent);
        }

        [Test]
        public void HandleMarkMessageAsReadQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            MarkMessageAsReadQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => noticeHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleMarkMessageAsReadQuery_NotNullQuery_CallMarkMessageAsReadService()
        {
            // Arrange
            var query = new MarkMessageAsReadQuery(CustId.ValidId, "1");

            // Action
            var viewModel = noticeHandler.Handle(query);

            // Assert
            noticeService
                .Received()
                .MarkMessageAsRead(
                    Arg.Is<int>(cid => cid == query.CustId),
                    Arg.Is<string>(mid => mid == query.MessageIds));
        }

        private IEnumerable<NoticeMessage> CreateNoticeMessagesData(byte msgType = 1, bool isSticky = false)
        {
            var noticeMessage = new NoticeMessage(1, "en", "cs", "ch", "jp", "th", "lt", "ko", "es",
                DateTime.Today, msgType, isSticky, DateTime.Today, "1", true);

            var noticeMessages = new List<NoticeMessage>();
            noticeMessages.Add(noticeMessage);

            return noticeMessages;
        }

        private void FakeReturnGetNoticeMessagesService(IEnumerable<NoticeMessage> noticeMessages)
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