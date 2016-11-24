namespace Alpha.Main.Tests.Criteria.Announcement
{
    using Main.Criteria.Announcement;
    using NUnit.Framework;

    [TestFixture]
    public class InsertPersonalMessageTargetCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var insertPersonalMessageTargetCriteria = new InsertPersonalMessageTargetCriteria();
            var expectedSettingKey = "Dot_Msg_SetUsrMessage";

            // Act
            var actualSettingKey = insertPersonalMessageTargetCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidMessageId_ReturnsTrue()
        {
            // Arrange
            var insertPersonalMessageTargetCriteria = new InsertPersonalMessageTargetCriteria();
            insertPersonalMessageTargetCriteria.MessageId = 0;

            // Act
            var actualIsValid = insertPersonalMessageTargetCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_ValidMessageId_ReturnsTrue()
        {
            // Arrange
            var insertPersonalMessageTargetCriteria = new InsertPersonalMessageTargetCriteria();
            insertPersonalMessageTargetCriteria.MessageId = 1;

            // Act
            var actualIsValid = insertPersonalMessageTargetCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
