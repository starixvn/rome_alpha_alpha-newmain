using Alpha.Main.Site.ViewModels.Security;
using NUnit.Framework;

namespace Alpha.Main.Site.Tests.ViewModels.Security
{
    [TestFixture]
    public class ResetOtpViewModelTests
    {
        [Test]
        public void ResetTitle_Always_ReturnsCorrectValue()
        {
            // Arrange
            var viewModel = new ResetOtpViewModel();
            viewModel.Username = "UserName";
            var expectedResult = "Remove OTP - UserName";

            // Act
            var actualResult = viewModel.ResetTitle;

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
