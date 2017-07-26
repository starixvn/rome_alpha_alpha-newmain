using Alpha.Main.Criteria.SecurityCode;
using NUnit.Framework;

namespace Alpha.Main.Tests.Criteria.SecurityCode
{
    [TestFixture]
    public class UpdateMainAccountSecurityCodeCounterCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateMainAccountSecurityCodeCounterCriteria();
            var expectedSettingKey = "Age_UpdateMainAccountSecurityCodeCounter";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnsTrue()
        {
            // Arrange
            var criteria = new UpdateMainAccountSecurityCodeCounterCriteria();
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
            var criteria = new UpdateMainAccountSecurityCodeCounterCriteria();
            criteria.CustId = 0;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
