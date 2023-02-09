namespace Alpha.Main.Tests.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Services.Impl;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class FeedbackServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private FeedbackServiceImpl service;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            service = new FeedbackServiceImpl(dynamicRepository);
            compareLogic = new CompareLogic();
        }

        [Test]
        public async Task CheckExistsAsync_Always_ReturnsData()
        {
            // Arrange
            var expectedResult = false;

            // Act
            var actualResult = await service.CheckExistsAsync(0);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
