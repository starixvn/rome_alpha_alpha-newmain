namespace Alpha.Main.Tests.Constants
{
    using System.Collections.Generic;
    using Main.Constants;
    using NUnit.Framework;

    [TestFixture]
    public class LanguageTests
    {
        [Test]
        public void GetCodeKeys_Always_ReturnsExpectedCodeKeys()
        {
            // Arrange
            var expectedCodeKeys = new Dictionary<string, string>
            {
                ["en-US"] = "E",
                ["zh-TW"] = "CT",
                ["zh-CN"] = "CS",
                ["ja-JP"] = "J",
                ["th-TH"] = "TH",
                ["ko-KR"] = "KO",
                ["vi-VN"] = "VI"
            };

            // Act
            var actualCodeKeys = Language.CodeKeys;

            // Assert
            Assert.AreEqual(expectedCodeKeys, actualCodeKeys);
        }

        [Test]
        public void GetCodeNames_Always_ReturnsExpectedCodeNames()
        {
            // Arrange
            var expectedCodeNames = new Dictionary<string, string>
            {
                ["en-US"] = "English",
                ["zh-TW"] = "繁體中文",
                ["zh-CN"] = "简体中文",
                ["ja-JP"] = "日本語",
                ["th-TH"] = "ภาษาไทย",
                ["ko-KR"] = "한국어",
                ["vi-VN"] = "Tiếng Việt"
            };

            // Act
            var actualCodeNames = Language.CodeNames;

            // Assert
            Assert.AreEqual(expectedCodeNames, actualCodeNames);
        }
    }
}
