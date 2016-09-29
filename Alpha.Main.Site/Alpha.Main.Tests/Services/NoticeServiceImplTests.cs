namespace Alpha.Main.Tests.Services
{
    using System;
    using Constants;
    using Fanex.Data.Repository;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

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
        public void GetNoticeMessages_InvalidCustIdAndParentSiteIds_ReturnNull()
        {
            // Action
            var noticeMessages = noticeService.GetNoticeMessages(
                CustId.InvalidId,
                string.Empty,
                DateTime.Today,
                10,
                false);

            // Assert
            Assert.AreEqual(noticeMessages, null);
        }
    }
}