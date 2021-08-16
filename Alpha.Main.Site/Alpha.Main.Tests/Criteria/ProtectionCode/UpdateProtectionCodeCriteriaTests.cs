namespace Alpha.Main.Tests.Criteria.ProtectionCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Alpha.Main.Criteria.ProtectionCode;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateProtectionCodeCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateProtectionCodeCriteria("123457", 1, 0, "E", "::1");
            var expectedSettingKey = "Age_CustomerUpdProtectionCode";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase("123457", true)]
        [TestCase("", false)]
        [TestCase("          ", false)]
        [TestCase(null, false)]
        public void IsValid_ProtectionCode_ReturnsExpectedValue(string protectionCode, bool expected)
        {
            // Arrange
            var criteria = new UpdateProtectionCodeCriteria(protectionCode, 1, 0, "E", "::1");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actualIsValid);
        }

        [TestCase(10, true)]
        [TestCase(0, false)]
        [TestCase(-100, false)]
        public void IsValid_CustId_ReturnsExpectedValue(int custId, bool expected)
        {
            // Arrange
            var criteria = new UpdateProtectionCodeCriteria("123457", custId, 0, "E", "::1");

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
            var criteria = new UpdateProtectionCodeCriteria("123457", 1, subAccId, "E", "::1");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actualIsValid);
        }
    }
}
