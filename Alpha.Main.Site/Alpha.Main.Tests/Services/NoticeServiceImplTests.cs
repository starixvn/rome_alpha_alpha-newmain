namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fanex.Data.Repository;
    using Main.Models.Notice;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class NoticeServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private NoticeServiceImpl noticeService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            noticeService = new NoticeServiceImpl(dynamicRepository);
        }

        [Test]
        public void GetNoticeMessages_InvalidCustIdAndParentSiteIds_ReturnEmpty()
        {
            // Act
            var noticeMessages = noticeService.GetNoticeMessages(
                CustId.InvalidId,
                string.Empty,
                DateTime.Today,
                10,
                false);

            // Assert
            Assert.AreEqual(noticeMessages, new List<NoticeMessage>());
        }

        [Test]
        public void GetNoticeMessages_ValidCustIdAndParentSiteIds_ReturnNoticeMessages()
        {
            // Arrange
            var expectedNoticeMessages = FakeDataHelper.CreateMany<NoticeMessage>().ToList();

            dynamicRepository.Fetch<NoticeMessage>(Arg.Any<ICriteria>()).Returns(expectedNoticeMessages);

            // Act
            var noticeMessages = noticeService.GetNoticeMessages(CustId.ValidId, "1", DateTime.Today, 10, false);

            // Assert
            Assert.AreEqual(noticeMessages, expectedNoticeMessages);
        }

        //[Test]
        //public void MarkMessageAsRead_InvalidCustId_ReturnFalse()
        //{
        //    // Act
        //    var isSuccess = noticeService.MarkMessageAsRead(CustId.InvalidId, 1);

        //    // Assert
        //    Assert.IsFalse(isSuccess);
        //}

        //[Test]
        //public void MarkMessageAsRead_ValidCustIdAndMessageIds_ReturnTrue()
        //{
        //    // Act
        //    var isSuccess = noticeService.MarkMessageAsRead(CustId.ValidId, 1);

        //    // Assert
        //    Assert.IsTrue(isSuccess);
        //}
    }
}