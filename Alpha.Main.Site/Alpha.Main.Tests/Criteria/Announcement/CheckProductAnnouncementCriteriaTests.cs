namespace Alpha.Main.Tests.Criteria.Announcement
{
    using Main.Criteria.Announcement;
    using NUnit.Framework;

    [TestFixture]
    public class CheckProductAnnouncementCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var checkProductAnnouncementCriteria = new CheckProductAnnouncementCriteria();
            var expectedSettingKey = "Age_CustomerShowAnnouncement";

            // Act
            var actualSettingKey = checkProductAnnouncementCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InValidCustId_ReturnsFalse()
        {
            // Arrange
            var checkProductAnnouncementCriteria = new CheckProductAnnouncementCriteria();
            checkProductAnnouncementCriteria.CustId = 0;

            // Act
            var actualIsValid = checkProductAnnouncementCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnsTrue()
        {
            // Arrange
            var checkProductAnnouncementCriteria = new CheckProductAnnouncementCriteria();
            checkProductAnnouncementCriteria.CustId = 1;

            // Act
            var actualIsValid = checkProductAnnouncementCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
