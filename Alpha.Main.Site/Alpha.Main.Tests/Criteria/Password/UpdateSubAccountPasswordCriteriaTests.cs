using Alpha.Main.Criteria.Password;
using NUnit.Framework;

namespace Alpha.Main.Tests.Criteria.Password
{
    [TestFixture]
    public class UpdateSubAccountPasswordCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateSubAccountPasswordCriteria();
            var expectedSettingKey = "UpdateSubAccountPassword";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnsTrue()
        {
            // Arrange
            var criteria = new UpdateSubAccountPasswordCriteria();
            criteria.CustId = 1;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Test]
        public void IsValid_InvalidCustId_ReturnsFalse()
        {
            // Arrange
            var criteria = new UpdateSubAccountPasswordCriteria();
            criteria.CustId = 0;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
