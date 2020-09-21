namespace Alpha.Main.Tests.Criteria.Banner
{
    using Alpha.Main.Criteria.Banner;
    using NUnit.Framework;

    [TestFixture]
    public class GetBannerCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnValidSettingKey()
        {
            // Arrange
            var criteria = new GetBannerCriteria("en-US", "Local", "IBCBET", 1);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_Banner_SelectRandomBanner", actualSettingKey);
        }

        [TestCase("en-US", true)]
        [TestCase("qwerty", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsValid_LanguageCode_ReturnsCorrectExpected(string languageCode, bool expected)
        {
            // Arrange
            var criteria = new GetBannerCriteria(languageCode, "Local", "IBCBET", 1);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Local", true)]
        [TestCase("qwerty", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsValid_Environments_ReturnsCorrectExpected(string environments, bool expected)
        {
            // Arrange
            var criteria = new GetBannerCriteria("en-US", environments, "IBCBET", 1);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("IBCBET", true)]
        [TestCase("qwerty", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsValid_Systems_ReturnsCorrectExpected(string systems, bool expected)
        {
            // Arrange
            var criteria = new GetBannerCriteria("en-US", "Local", systems, 1);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(byte.MaxValue, false)]
        [TestCase(byte.MinValue, false)]
        public void IsValid_Type_ReturnsCorrectExpected(byte type, bool expected)
        {
            // Arrange
            var criteria = new GetBannerCriteria("en-US", "Local", "IBCBET", type);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
