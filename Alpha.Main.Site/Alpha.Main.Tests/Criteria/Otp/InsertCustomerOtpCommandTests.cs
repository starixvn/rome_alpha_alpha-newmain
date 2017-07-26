using Alpha.Main.Criteria.Otp;
using NUnit.Framework;

namespace Alpha.Main.Tests.Criteria.Otp
{
    [TestFixture]
    public class InsertCustomerOtpCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var insertCustomerOtpCommand = new InsertCustomerOtpCommand();
            var expectedSettingKey = "Age_OTP_CustomerOTPIns";

            // Act
            var actualSettingKey = insertCustomerOtpCommand.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValuesAreNotNullOrWhiteSpace_ReturnsTrue()
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";
            command.DeviceId = "DeviceId";
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_CustIdIsLessThanOrEqualZero_ReturnsFalse(int custId)
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = custId;
            command.SubAccId = 0;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";
            command.DeviceId = "DeviceId";
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Test]
        public void IsValid_SubAccIdIsLessThanZero_ReturnsFalse()
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = -1;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";
            command.DeviceId = "DeviceId";
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_LanguageIsNullOrWhiteSpace_ReturnsFalse(string language)
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.Language = language;
            command.DeviceInfo = "DeviceInfo";
            command.IP = "IP";
            command.DeviceId = "DeviceId";
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_IPIsNullOrWhiteSpace_ReturnsFalse(string ip)
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.Language = "Language";
            command.IP = ip;
            command.DeviceInfo = "DeviceInfo";
            command.DeviceId = "DeviceId";
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_DeviceInfoIsNullOrWhiteSpace_ReturnsFalse(string deviceInfo)
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.Language = "Language";
            command.IP = "IP";
            command.DeviceInfo = deviceInfo;
            command.DeviceId = "DeviceId";
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_DeviceIdIsNullOrWhiteSpace_ReturnsFalse(string deviceId)
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.Language = "Language";
            command.IP = "IP";
            command.DeviceInfo = "DeviceInfo";
            command.DeviceId = deviceId;
            command.SecretKey = "SecretKey";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_SecretKeyIsNullOrWhiteSpace_ReturnsFalse(string secretKey)
        {
            // Arrange
            var command = new InsertCustomerOtpCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.Language = "Language";
            command.IP = "IP";
            command.DeviceInfo = "DeviceInfo";
            command.DeviceId = "DeviceId";
            command.SecretKey = secretKey;

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
