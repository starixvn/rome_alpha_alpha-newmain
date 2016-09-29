namespace Alpha.Main.Tests.Services
{
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
    }
}