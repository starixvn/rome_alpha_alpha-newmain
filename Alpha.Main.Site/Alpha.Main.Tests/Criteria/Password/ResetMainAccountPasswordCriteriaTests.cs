using Alpha.Main.Criteria.Password;
using NUnit.Framework;

namespace Alpha.Main.Tests.Criteria.Password
{
    [TestFixture]
    public class ResetMainAccountPasswordCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var resetMainAccountPasswordCriteria = new ResetMainAccountPasswordCriteria();
            var expectedSettingKey = "ResetMainAccountPassword";

            // Act
            var actualSettingKey = resetMainAccountPasswordCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnsTrue()
        {
            // Arrange
            var criteria = new ResetMainAccountPasswordCriteria();
            criteria.CustId = 1;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Test]
        public void IsValid_InValidCustId_ReturnsFalse()
        {
            // Arrange
            var criteria = new ResetMainAccountPasswordCriteria();
            criteria.CustId = 0;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
