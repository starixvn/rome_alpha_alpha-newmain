namespace Alpha.Main.Tests.Criteria.Announcement
{
    using Main.Criteria.Announcement;
    using NUnit.Framework;

    [TestFixture]
    public class GetMessageTypeListCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getMessageTypeListCriteria = new GetMessageTypeListCriteria(0);
            var expectedSettingKey = "Age_MessageNoticeSelByCustID";

            // Act
            var actualSettingKey = getMessageTypeListCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnsFalse()
        {
            // Arrange
            var getMessageTypeListCriteria = new GetMessageTypeListCriteria(0);

            // Act
            var actualIsValid = getMessageTypeListCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnsTrue()
        {
            // Arrange
            var getMessageTypeListCriteria = new GetMessageTypeListCriteria(1);

            // Act
            var actualIsValid = getMessageTypeListCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
