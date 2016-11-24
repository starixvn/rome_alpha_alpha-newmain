namespace Alpha.Main.Tests.Criteria.Announcement
{
    using Main.Criteria.Announcement;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateViewStatusTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var updateViewStatus = new UpdateViewStatus(0, false);
            var expectedSettingKey = "Age_MessageNoticeUpdStatus";

            // Act
            var actualSettingKey = updateViewStatus.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnsFalse()
        {
            // Arrange
            var updateViewStatus = new UpdateViewStatus(0, false);

            // Act
            var actualIsValid = updateViewStatus.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnsTrue()
        {
            // Arrange
            var updateViewStatus = new UpdateViewStatus(1, false);

            // Act
            var actualIsValid = updateViewStatus.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
