namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using Fanex.RemotingRef;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CacheRefServiceImplTests
    {
        private const string DefaultLanguage = "en-US";
        private IRemotingRefService remotingRefService;
        private CacheRefService cacheRefService;

        [SetUp]
        public void SetUp()
        {
            remotingRefService = Substitute.For<IRemotingRefService>();
            cacheRefService = new CacheRefServiceImpl(remotingRefService);
        }

        [Test]
        public void GetSportName_ValidSportTypeId_ReturnsCorrectSportName()
        {
            // Arrange
            var expectedResult = "Virtual Sports";
            var sportTypeId = 5;
            remotingRefService
                .GetSportName(sportTypeId.ToString(), DefaultLanguage)
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetSportName(sportTypeId);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetProductName_ValidProductId_ReturnsCorrectProductName()
        {
            // Arrange
            var expectedResult = "Virtual Sports";
            var productId = 5;
            remotingRefService
                .GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), DefaultLanguage, string.Empty)
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetProductName(productId);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetProductName_InvalidProductId_ReturnsDefaultProductName()
        {
            // Arrange
            var expectedResult = string.Empty;
            var productId = -5;
            remotingRefService
                .GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), DefaultLanguage, expectedResult)
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetProductName(productId);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
