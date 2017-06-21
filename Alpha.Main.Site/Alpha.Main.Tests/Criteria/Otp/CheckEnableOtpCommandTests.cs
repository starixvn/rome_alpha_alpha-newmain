namespace Alpha.Main.Tests.Criteria.Otp
{
    using Alpha.Main.Criteria.Otp;
    using NUnit.Framework;

    [TestFixture]
    public class CheckEnableOtpCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var checkEnableOtpCommand = new CheckEnableOtpCommand();
            var expectedSettingKey = "Age_OTP_CustomerCheckEnableOTP";

            // Act
            var actualSettingKey = checkEnableOtpCommand.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(1, -1)]
        public void IsValid_CustIdIsLessThanOrEqualZeroOrSubAccIdIsLessThanZero_ReturnsFalse(int custId, int subAccId)
        {
            // Arrange
            var checkEnableOtpCommand = new CheckEnableOtpCommand();
            checkEnableOtpCommand.CustId = custId;
            checkEnableOtpCommand.SubAccId = subAccId;

            // Act
            var actualIsValid = checkEnableOtpCommand.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void IsValid_CustIdIsGreaterThanZeroOrSubAccIdIsGreaterThanOrEqualZero_ReturnsTrue(int custid, int subAccId)
        {
            // Arrange
            var checkEnableOtpCommand = new CheckEnableOtpCommand();
            checkEnableOtpCommand.CustId = custid;
            checkEnableOtpCommand.SubAccId = subAccId;

            // Act
            var actualIsValid = checkEnableOtpCommand.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
