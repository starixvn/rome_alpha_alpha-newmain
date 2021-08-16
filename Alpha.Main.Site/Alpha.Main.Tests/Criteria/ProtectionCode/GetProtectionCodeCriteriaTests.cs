namespace Alpha.Main.Tests.Criteria.ProtectionCode
{
    using Alpha.Main.Criteria.ProtectionCode;
    using NUnit.Framework;

    [TestFixture]
    public class GetProtectionCodeCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetProtectionCodeCriteria(1, 0);
            var expectedSettingKey = "Age_CustomerSelProtectionCode";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(10, true)]
        [TestCase(0, false)]
        [TestCase(-100, false)]
        public void IsValid_CustId_ReturnsExpectedValue(int custId, bool expected)
        {
            // Arrange
            var criteria = new GetProtectionCodeCriteria(custId, 0);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actualIsValid);
        }

        [TestCase(10, true)]
        [TestCase(0, true)]
        [TestCase(-100, false)]
        public void IsValid_SubAccId_ReturnsExpectedValue(int subAccId, bool expected)
        {
            // Arrange
            var criteria = new GetProtectionCodeCriteria(1, subAccId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actualIsValid);
        }
    }
}
