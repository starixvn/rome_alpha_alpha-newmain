namespace Alpha.Main.Tests.Helpers
{
    using System;
    using Main.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class AppPathHelperTests
    {
        [Test]
        public void GetAppPath_WithAppNameAndVirtualPath_ReturnsEmpty()
        {
            // Arrange
            var validAppName = "ValidAppName";
            var validVirtualPath = "ValidVirtualPath";

            // Act
            var actualAppPath = AppPathHelper.GetAppPath(validAppName, validVirtualPath);

            // Assert
            Assert.IsEmpty(actualAppPath);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        public void GetAppPath_WithEmptyAppName_ThrowsArgumentException(string appName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(new TestDelegate(() => AppPathHelper.GetAppPath(appName, "1")));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        public void GetAppPath_WithEmptyVirtualPath_ThrowsArgumentException(string virtualPath)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(new TestDelegate(() => AppPathHelper.GetAppPath("1", virtualPath)));
        }
    }
}