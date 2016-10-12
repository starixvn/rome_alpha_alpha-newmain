namespace Alpha.Main.Site.Tests.ViewModels.Message
{
    using System;
    using Constants;
    using Main.Models.Message;
    using NUnit.Framework;
    using Site.ViewModels.Message;

    [TestFixture]
    public class MessageWrapperTests
    {
        [TestCase(Language.ChineseSimplified, "en")]
        [TestCase(Language.ChineseTraditional, "en")]
        [TestCase(Language.Thai, "en")]
        [TestCase(Language.Japanese, "en")]
        [TestCase(Language.Korean, "en")]
        [TestCase(Language.Vietnamese, "en")]
        [TestCase(Language.English, "en")]
        public void Content_WhenCurrentLanguageContentIsNull_ReturnContentDefault(string lang, string content)
        {
            // Arrange
            var message = CreateMessageDataWithDefaultContent();

            // Act
            var viewModel = new MessageWrapper(message, lang);

            // Assert
            Assert.AreEqual(content, viewModel.Content);
        }

        [TestCase(Language.ChineseSimplified, "cs")]
        [TestCase(Language.ChineseTraditional, "ch")]
        [TestCase(Language.Thai, "th")]
        [TestCase(Language.Japanese, "jp")]
        [TestCase(Language.Korean, "ko")]
        [TestCase(Language.Vietnamese, "en")]
        [TestCase(Language.English, "en")]
        public void Content_ByCurrentLanguage_ReturnContent(string lang, string content)
        {
            // Arrange
            var message = CreateMessageData();

            // Act
            var viewModel = new MessageWrapper(message, lang);

            // Assert
            Assert.AreEqual(content, viewModel.Content);
        }

        [Test]
        public void Content_HasFontMarkup_RemoveMarkup()
        {
            // Arrange
            var message = CreateMessageDataWithDefaultContent("<font color=red>abc</font>");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.AreEqual("abc", viewModel.Content);
        }

        [Test]
        public void Content_HasDoubleAtSymbol_ReplaceWithBetTransId()
        {
            // Arrange
            var message = CreateMessageDataWithDefaultContent("abc@@");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.AreEqual("abc1", viewModel.Content);
        }

        [Test]
        public void HasRedColor_ContentHasFontMarkup_ReturnTrue()
        {
            // Arrange
            var message = CreateMessageDataWithDefaultContent("<font color=red>abc</font>");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.IsTrue(viewModel.HasRedColor);
        }

        [Test]
        public void HasRedColor_ContentHasNotFontMarkup_ReturnFalse()
        {
            // Arrange
            var message = CreateMessageDataWithDefaultContent("abc");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.IsFalse(viewModel.HasRedColor);
        }

        private Message CreateMessageData()
        {
            var message = new Message(
                1,
                "en",
                "cs",
                "ch",
                "jp",
                "th",
                "it",
                "ko",
                "es",
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                1,
                false,
                false,
                1,
                true,
                1,
                1,
                "name",
                "name",
                false,
                "1",
                1,
                "name",
                1);

            return message;
        }

        private Message CreateMessageDataWithDefaultContent(string contentDefault = "en")
        {
            var message = new Message(
                1,
                contentDefault,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                1,
                false,
                false,
                1,
                true,
                1,
                1,
                "name",
                "name",
                false,
                "1",
                1,
                "name",
                1);

            return message;
        }
    }
}