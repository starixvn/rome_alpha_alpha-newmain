namespace Alpha.Main.Tests.Criteria.Otp
{
    using Alpha.Main.Criteria.Otp;
    using NUnit.Framework;

    [TestFixture]
    public class GetDevicesCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getDevicesCriteria = new GetDevicesCriteria();
            var expectedSettingKey = "Age_OTP_CustomerOTPDeviceInfoSel";

            // Act
            var actualSettingKey = getDevicesCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void IsValid_ValidCustIdAndValidSubAccId_ReturnsTrue(int custId, int subAccId)
        {
            // Arrange
            var getDevicesCriteria = new GetDevicesCriteria();
            getDevicesCriteria.CustId = custId;
            getDevicesCriteria.SubAccId = subAccId;

            // Act
            var actualIsValid = getDevicesCriteria.IsValid();

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
            var getDevicesCriteria = new GetDevicesCriteria();
            getDevicesCriteria.CustId = custId;
            getDevicesCriteria.SubAccId = subAccId;

            // Act
            var actualIsValid = getDevicesCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
