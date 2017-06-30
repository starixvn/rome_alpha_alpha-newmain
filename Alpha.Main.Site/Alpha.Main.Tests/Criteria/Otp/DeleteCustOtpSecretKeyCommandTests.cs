using Alpha.Main.Criteria.Otp;
using NUnit.Framework;

namespace Alpha.Main.Tests.Criteria.Otp
{
    [TestFixture]
    public class DeleteCustOtpSecretKeyCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var deleteCustOtpSecretKeyCommand = new DeleteCustOtpSecretKeyCommand();
            var expectedSettingKey = "Age_OTP_CustomerOTPDel";

            // Act
            var actualSettingKey = deleteCustOtpSecretKeyCommand.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValuesAreNotNullOrWhiteSpace_ReturnsTrue()
        {
            // Arrange
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = 1;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = custId;
            command.SubAccId = 0;
            command.UserId = 1;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Test]
        public void IsValid_InvalidSubAccId_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = 1;
            command.SubAccId = -1;
            command.UserId = 1;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidUserId_ReturnsFalse(int userId)
        {
            // Arrange
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = userId;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = "IP";

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
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = 1;
            command.DeviceInfo = deviceInfo;
            command.Language = "Language";
            command.IP = "IP";

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
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = 1;
            command.DeviceInfo = "DeviceInfo";
            command.Language = language;
            command.IP = "IP";

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
            var command = new DeleteCustOtpSecretKeyCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = 1;
            command.DeviceInfo = "DeviceInfo";
            command.Language = "Language";
            command.IP = ip;

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
