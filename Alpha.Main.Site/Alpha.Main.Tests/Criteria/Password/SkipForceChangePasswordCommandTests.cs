namespace Alpha.Main.Tests.Criteria.Password
{
    using Alpha.Main.Criteria.Password;
    using NUnit.Framework;

    [TestFixture]
    public class SkipForceChangePasswordCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SkipForceChangePasswordCommand();
            var expectedSettingKey = "Age_CustomerSkipForceUpdPwd";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(10, true)]
        [TestCase(0, false)]
        [TestCase(-10, false)]
        public void IsValid_CustId_ReturnsCorrectExpectation(int custId, bool expected)
        {
            // Arrange
            var criteria = new SkipForceChangePasswordCommand
            {
                CustId = custId
            };

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actualIsValid);
        }

        [Test]
        public void Property_Always_AssignsCorrectProperties()
        {
            // Arrange & Act
            var criteria = new SkipForceChangePasswordCommand
            {
                CustId = 123456,
                SubAccId = 789,
                LoginIP = "::1",
                Language = "E",
                ErrCode = 0,
                ErrMsg = string.Empty,
            };

            // Assert
            Assert.IsTrue(123456 == criteria.CustId
                && 789 == criteria.SubAccId
                && "::1" == criteria.LoginIP
                && "E" == criteria.Language
                && 0 == criteria.ErrCode
                && string.Empty == criteria.ErrMsg);
        }
    }
}
