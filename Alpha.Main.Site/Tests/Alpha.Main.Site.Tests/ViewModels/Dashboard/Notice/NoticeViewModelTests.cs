namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using Main.Constants;
    using Main.Models.Notice;
    using NUnit.Framework;
    using Site.ViewModels.Dashboard.Notice;

    [TestFixture]
    public class NoticeViewModelTests
    {
        [Test]
        public void LastMessagePostTime_NoticeMessagesHasNoData_ReturnMinDate()
        {
            // Arrange
            var noticeMessaseWrappers = new List<NoticeMessageWrapper>();

            // Act
            var viewModel = new NoticeViewModel(noticeMessaseWrappers);

            // Assert
            Assert.AreEqual(DateTime.MinValue, viewModel.LastMessagePostTime);
        }

        [Test]
        public void LastMessagePostTime_NoticeMessagesHasData_ReturnLastPostDate()
        {
            // Arrange
            var noticeMessaseWrappers = new List<NoticeMessageWrapper>();

            noticeMessaseWrappers.Add(CreateNoticeMessageWrapper(DateTime.Today));
            noticeMessaseWrappers.Add(CreateNoticeMessageWrapper(DateTime.Today.AddDays(1)));

            // Act
            var viewModel = new NoticeViewModel(noticeMessaseWrappers);

            // Assert
            Assert.AreEqual(DateTime.Today, viewModel.LastMessagePostTime);
        }

        private NoticeMessageWrapper CreateNoticeMessageWrapper(
            DateTime postTime,
            byte msgType = 1,
            bool isSticky = false)
        {
            var noticeMessage = new NoticeMessage(
                1,
                "en",
                "cs",
                "ch",
                "jp",
                "th",
                "lt",
                "ko",
                "es",
                postTime,
                msgType,
                isSticky,
                DateTime.Today,
                "1",
                true);

            var noticeMessageWrapper = new NoticeMessageWrapper(noticeMessage, Language.Default);

            return noticeMessageWrapper;
        }
    }
}