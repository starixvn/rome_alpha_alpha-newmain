namespace Alpha.Main.Tests.Criteria.SecurityCode
{
    using Alpha.Main.Criteria.SecurityCode;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateSecurityCodeFunctionCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateSecurityCodeFunctionCriteria(true, "BetList", 1, 0, "E", "::1");
            var expectedSettingKey = "Age_CustomerUpdSecurityFunctionInfo";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(100, true)]
        [TestCase(0, false)]
        [TestCase(-1000, false)]
        public void IsValid_CustId_ReturnsCorrectExpected(int custId, bool expected)
        {
            // Arrange
            var criteria = new UpdateSecurityCodeFunctionCriteria(true, "BetList", custId, 0, "E", "::1");

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(100, true)]
        [TestCase(0, true)]
        [TestCase(-1000, false)]
        public void IsValid_SubAccId_ReturnsCorrectExpected(int subAccId, bool expected)
        {
            // Arrange
            var criteria = new UpdateSecurityCodeFunctionCriteria(true, "BetList", 1, subAccId, "E", "::1");

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
