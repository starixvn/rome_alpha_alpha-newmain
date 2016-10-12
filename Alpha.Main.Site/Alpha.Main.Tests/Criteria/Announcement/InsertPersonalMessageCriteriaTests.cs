namespace Alpha.Main.Tests.Criteria.Announcement
{
    using Main.Criteria.Announcement;
    using NUnit.Framework;

    [TestFixture]
    public class InsertPersonalMessageCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var insertPersonalMessageCriteria = new InsertPersonalMessageCriteria();
            var expectedSettingKey = "Dot_Msg_SetMessage";

            // Act
            var actualSettingKey = insertPersonalMessageCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var insertPersonalMessageCriteria = new InsertPersonalMessageCriteria();

            // Act
            var actualIsValid = insertPersonalMessageCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
