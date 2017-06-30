using Alpha.Main.Criteria.Password;
using NUnit.Framework;

namespace Alpha.Main.Tests.Criteria.Password
{
    [TestFixture]
    public class ResetSubAccountPasswordCriteriaTests
    {
        [Test]
        public void LoginSubAccId_Always_ReturnsZero()
        {
            // Arrange
            var criteria = new ResetSubAccountPasswordCriteria();
            var expectedLoginSubAccId = 0;

            // Act
            var actualLoginSubAccId = criteria.LoginSubAccId;

            // Assert
            Assert.AreEqual(expectedLoginSubAccId, actualLoginSubAccId);
        }

        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new ResetSubAccountPasswordCriteria();
            var expectedSettingKey = "ResetSubAccountPassword";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnsTrue()
        {
            // Arrange
            var criteria = new ResetSubAccountPasswordCriteria();
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
            var criteria = new ResetSubAccountPasswordCriteria();
            criteria.CustId = 0;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
