namespace Alpha.Main.Tests.Criteria.Otp
{
    using Alpha.Main.Criteria.Otp;
    using NUnit.Framework;

    [TestFixture]
    public class GetCustomerOtpCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getCustomerOtpCriteria = new GetCustomerOtpCriteria();
            var expectedSettingKey = "Age_OTP_CustomerOTPSel";

            // Act
            var actualSettingKey = getCustomerOtpCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void IsValid_ValidCustIdAndValidSubAccId_ReturnsTrue(int custId, int subAccId)
        {
            // Arrange
            var getCustomerOtpCriteria = new GetCustomerOtpCriteria();
            getCustomerOtpCriteria.CustId = custId;
            getCustomerOtpCriteria.SubAccId = subAccId;

            // Act
            var actualIsValid = getCustomerOtpCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(1, -1)]
        public void IsValid_InvalidCustIdOrInvalidSubAccId_ReturnsFalse(int custId, int subAccId)
        {
            // Arrange
            var getCustomerOtpCriteria = new GetCustomerOtpCriteria();
            getCustomerOtpCriteria.CustId = custId;
            getCustomerOtpCriteria.SubAccId = subAccId;

            // Act
            var actualIsValid = getCustomerOtpCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
