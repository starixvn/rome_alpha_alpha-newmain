namespace Alpha.Main.Tests.Criteria.Theme
{
    using Alpha.Main.Criteria.Theme;
    using NUnit.Framework;

    [TestFixture]
    public class LogChangeThemeCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnValidSettingKey()
        {
            // Arrange
            var criteria = new LogChangeThemeCriteria(1, 2, true, "::1", "HeaderMenu");

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_SwitchDarkMode", actualSettingKey);
        }

        [TestCase(10, true)]
        [TestCase(0, false)]
        [TestCase(-10, false)]
        public void IsValid_CustId_ReturnsCorrectExpected(int custId, bool expected)
        {
            // Arrange
            var criteria = new LogChangeThemeCriteria(custId, 2, true, "::1", "HeaderMenu");

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(10, true)]
        [TestCase(0, true)]
        [TestCase(-10, false)]
        public void IsValid_SubAccId_ReturnsCorrectExpected(int subAccId, bool expected)
        {
            // Arrange
            var criteria = new LogChangeThemeCriteria(1, subAccId, true, "::1", "HeaderMenu");

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
