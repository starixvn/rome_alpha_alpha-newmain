namespace Alpha.Main.Tests.Criteria.Language
{
    using Alpha.Main.Criteria.Language;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateUsageLanguageCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var command = new UpdateUsageLanguageCommand(1, 0, "E", "::1");
            var expectedSettingKey = "Age_CustLanguageUsageUpd";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(100, true)]
        [TestCase(0, false)]
        [TestCase(-100, false)]
        public void IsValid_CustId_ReturnsExpectedResult(int custId, bool expectedResult)
        {
            // Arrange
            var command = new UpdateUsageLanguageCommand(custId, 0, "E", "::1");

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualIsValid);
        }

        [TestCase(100, true)]
        [TestCase(0, true)]
        [TestCase(-100, false)]
        public void IsValid_SubAccId_ReturnsExpectedResult(int subAccId, bool expectedResult)
        {
            // Arrange
            var command = new UpdateUsageLanguageCommand(100, subAccId, "E", "::1");

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualIsValid);
        }

        [TestCase("E", true)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        [TestCase(null, false)]
        public void IsValid_LanguageCode_ReturnsExpectedResult(string languageCode, bool expectedResult)
        {
            // Arrange
            var command = new UpdateUsageLanguageCommand(100, 0, languageCode, "::1");

            // Act
            var actualIsValid = command.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualIsValid);
        }
    }
}
