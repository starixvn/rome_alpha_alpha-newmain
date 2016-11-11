namespace Alpha.Main.Tests.Criteria.Notice
{
    using Main.Criteria.Notice;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MarkMessageAsReadCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_Return_Age_CustomerUpdMessagesRead()
        {
            // Arrange
            var criteria = new MarkMessageAsReadCriteria(1, "1");

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_CustomerUpdMessagesRead", actualSettingKey);
        }

        [Test]
        public void IsValid_UserIdIsGreaterThanZero_ReturnTrue()
        {
            // Arrange
            var criteria = new MarkMessageAsReadCriteria(69, "1");

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(true, actual);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_UserIdIsLessThanOrEqualZero_ReturnFalse(int userId)
        {
            // Arrange
            var criteria = new MarkMessageAsReadCriteria(userId, "1");

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void UserId_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var userId = 69;
            var criteria = new MarkMessageAsReadCriteria(userId, "1");

            // Act
            var actualUserId = criteria.UserId;

            // Assert
            Assert.AreEqual(userId, actualUserId);
        }

        [Test]
        public void MessageIds_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var messageIds = "3000";
            var criteria = new MarkMessageAsReadCriteria(69, messageIds);

            // Act
            var actualMessageIds = criteria.MessageIds;

            // Assert
            Assert.AreEqual(messageIds, actualMessageIds);
        }
    }
}