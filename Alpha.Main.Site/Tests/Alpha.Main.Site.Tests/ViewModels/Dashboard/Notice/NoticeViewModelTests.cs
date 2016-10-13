namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Tests.TestData.Helper;
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
            var viewModel = new NoticeMessagesViewModel(noticeMessaseWrappers);

            // Assert
            Assert.AreEqual(DateTime.MinValue, viewModel.LastMessagePostTime);
        }

        [Test]
        public void LastMessagePostTime_NoticeMessagesHasData_ReturnLastPostDate()
        {
            // Arrange
            var noticeMessaseWrappers = FakeDataHelper.CreateMany<NoticeMessageWrapper>().ToList();
            var lastPostTime = noticeMessaseWrappers.OrderByDescending(m => m.CurrentMessage.PostTime).Last().PostTime;

            // Act
            var viewModel = new NoticeMessagesViewModel(noticeMessaseWrappers);

            // Assert
            Assert.AreEqual(lastPostTime, viewModel.LastMessagePostTime);
        }
    }
}