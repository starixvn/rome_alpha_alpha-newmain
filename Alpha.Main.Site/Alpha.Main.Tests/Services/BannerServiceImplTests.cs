namespace Alpha.Main.Tests.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Criteria.Banner;
    using Alpha.Main.Models.Banner;
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BannerServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private BannerService bannerService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            bannerService = new BannerServiceImpl(dynamicRepository);
        }

        [Test]
        public async Task GetBanner_Always_ReturnsCorrectBannerItem()
        {
            // Arrange
            dynamicRepository
                .GetAsync<BannerItem>(Arg.Any<GetBannerCriteria>())
                .Returns(new BannerItem(null));

            // Act
            var actualResult = await bannerService.GetBanner("en-US", 1);

            // Assert
            Assert.IsNull(actualResult.ImageContent);
        }
    }
}
