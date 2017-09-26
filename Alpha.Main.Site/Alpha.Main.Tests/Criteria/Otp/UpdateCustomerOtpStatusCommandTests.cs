namespace Alpha.Main.Tests.Criteria.Otp
{
    using Alpha.Main.Criteria.Otp;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateCustomerOtpStatusCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var updateCustomerOtpStatusCommand = new UpdateCustomerOtpStatusCommand();
            var expectedSettingKey = "Age_OTP_CustomerOTPStatusUpd";

            // Act
            var actualSettingKey = updateCustomerOtpStatusCommand.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValuesAreNotNullOrWhiteSpace_ReturnsTrue()
        {
            // Arrange
            var command = new UpdateCustomerOtpStatusCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = 1;
            command.Language = "Language";
            command.IP = "IP";

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
            var command = new UpdateCustomerOtpStatusCommand();
            command.CustId = custId;
            command.SubAccId = 0;
            command.Language = "Language";
            command.IP = "IP";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Test]
        public void IsValid_SubAccIdIsLessThanZero_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCustomerOtpStatusCommand();
            command.CustId = 1;
            command.SubAccId = -1;
            command.Language = "Language";
            command.IP = "IP";

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_UserIdIsLessThanOrEqualZero_ReturnsFalse(int userId)
        {
            // Arrange
            var command = new UpdateCustomerOtpStatusCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.UserId = userId;
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
            var command = new UpdateCustomerOtpStatusCommand();
            command.CustId = 1;
            command.SubAccId = 0;
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
            var command = new UpdateCustomerOtpStatusCommand();
            command.CustId = 1;
            command.SubAccId = 0;
            command.Language = "Language";
            command.IP = ip;

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
