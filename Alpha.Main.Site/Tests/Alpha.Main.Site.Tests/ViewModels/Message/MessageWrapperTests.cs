namespace Alpha.Main.Site.Tests.ViewModels.Message
{
    using System.Collections.Generic;
    using Constants;
    using Main.Models.Message;
    using Main.Tests.TestData.Helper;
    using NUnit.Framework;
    using Site.ViewModels.Message;

    [TestFixture]
    public class MessageWrapperTests
    {
        [TestCase(Language.ChineseSimplified, "contentCS")]
        [TestCase(Language.ChineseTraditional, "contentCH")]
        [TestCase(Language.Thai, "contentTH")]
        [TestCase(Language.Japanese, "contentJP")]
        [TestCase(Language.Korean, "contentKO")]
        [TestCase(Language.Vietnamese, "contentVN")]
        [TestCase(Language.English, "contentEN")]
        public void Content_WhenCurrentLanguageContentIsNull_ReturnContentDefault(string lang, string contentParamName)
        {
            // Arrange
            var parameters = new Dictionary<string, dynamic>
            {
                [contentParamName] = string.Empty,
                ["contentEN"] = "en"
            };

            var message = FakeDataHelper.Create<Message>(parameters);

            // Act
            var viewModel = new MessageWrapper(message, lang);

            // Assert
            Assert.AreEqual("en", viewModel.Content);
        }

        [TestCase(Language.ChineseSimplified, "contentCS")]
        [TestCase(Language.ChineseTraditional, "contentCH")]
        [TestCase(Language.Thai, "contentTH")]
        [TestCase(Language.Japanese, "contentJP")]
        [TestCase(Language.Korean, "contentKO")]
        [TestCase(Language.Vietnamese, "contentVN")]
        [TestCase(Language.English, "contentEN")]
        public void Content_ByCurrentLanguage_ReturnContent(string lang, string contentParamName)
        {
            // Arrange
            var message = FakeDataHelper.Create<Message>(contentParamName, contentParamName);

            // Act
            var viewModel = new MessageWrapper(message, lang);

            // Assert
            Assert.AreEqual(contentParamName, viewModel.Content);
        }

        [Test]
        public void Content_HasFontMarkup_RemoveMarkup()
        {
            // Arrange
            var message = FakeDataHelper.Create<Message>("contentEN", "<font color=red>abc</font>");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.AreEqual("abc", viewModel.Content);
        }

        [Test]
        public void Content_HasDoubleAtSymbol_ReplaceWithBetTransId()
        {
            // Arrange
            var message = FakeDataHelper.Create<Message>("contentEN", "abc@@");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.AreEqual("abc" + viewModel.CurrentMessage.BetTransId, viewModel.Content);
        }

        [Test]
        public void HasRedColor_ContentHasFontMarkup_ReturnTrue()
        {
            // Arrange
            var message = FakeDataHelper.Create<Message>("contentEN", "<font color=red>abc</font>");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.IsTrue(viewModel.HasRedColor);
        }

        [Test]
        public void HasRedColor_ContentHasNotFontMarkup_ReturnFalse()
        {
            // Arrange
            var message = FakeDataHelper.Create<Message>("contentEN", "abc");

            // Act
            var viewModel = new MessageWrapper(message, Language.English);

            // Assert
            Assert.IsFalse(viewModel.HasRedColor);
        }
    }
}