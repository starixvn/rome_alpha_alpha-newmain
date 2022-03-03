namespace Alpha.Main.Tests.Criteria.Password
{
    using Alpha.Main.Criteria.Password;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateLastChangePasswordIndirectAccountCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateLastChangePasswordIndirectAccountCriteria();
            var expectedSettingKey = "Age_CustomerIndirectLoginAccountUpd";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(10, 0)]
        [TestCase(10, -10)]
        [TestCase(0, 10)]
        [TestCase(-10, 10)]
        public void IsValid_ValidCustIdOrSubAccId_ReturnsTrue(int custId, int subAccId)
        {
            // Arrange
            var criteria = new UpdateLastChangePasswordIndirectAccountCriteria
            {
                CustId = custId,
                SubAccId = subAccId,
                CustIdIndirect = 100,
                SubAccIdIndirect = 0
            };

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [TestCase(0, 0)]
        [TestCase(0, -10)]
        [TestCase(-10, 0)]
        [TestCase(-10, -10)]
        public void IsValid_InvalidCustIdAndSubAccId_ReturnsFalse(int custId, int subAccId)
        {
            // Arrange
            var criteria = new UpdateLastChangePasswordIndirectAccountCriteria
            {
                CustId = custId,
                SubAccId = subAccId,
                CustIdIndirect = 100,
                SubAccIdIndirect = 0
            };

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(100, 0)]
        [TestCase(100, -100)]
        [TestCase(0, 100)]
        [TestCase(-100, 100)]
        public void IsValid_ValidCustIdIndirectOrSubAccIdIndirect_ReturnsTrue(int custIdIndirect, int subAccIdIndirect)
        {
            // Arrange
            var criteria = new UpdateLastChangePasswordIndirectAccountCriteria
            {
                CustId = 10,
                SubAccId = 0,
                CustIdIndirect = custIdIndirect,
                SubAccIdIndirect = subAccIdIndirect
            };

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [TestCase(0, 0)]
        [TestCase(0, -100)]
        [TestCase(-100, 0)]
        [TestCase(-100, -100)]
        public void IsValid_InvalidCustIdIndirectOrSubAccIdIndirect_ReturnsFalse(int custIdIndirect, int subAccIdIndirect)
        {
            // Arrange
            var criteria = new UpdateLastChangePasswordIndirectAccountCriteria
            {
                CustId = 10,
                SubAccId = 0,
                CustIdIndirect = custIdIndirect,
                SubAccIdIndirect = subAccIdIndirect
            };

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
