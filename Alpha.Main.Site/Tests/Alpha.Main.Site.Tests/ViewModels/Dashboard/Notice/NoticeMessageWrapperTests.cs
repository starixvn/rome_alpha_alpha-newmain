namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using System.Collections.Generic;
    using Main.Constants;
    using Main.Models.Notice;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Site.ViewModels.Dashboard.Notice;

    [TestFixture]
    public class NoticeMessageWrapperTests
    {
        [TestCase(1, false, "private")]
        [TestCase(1, true, "special")]
        [TestCase(3, true, "special")]
        [TestCase(3, false, "public")]
        [TestCase(4, false, "")]
        public void MessageType_ByMessageTypeAndIsSticky_ReturnTypeText(byte msgType, bool isSticky, string expectedType)
        {
            // Arrange
            var parameters = new Dictionary<string, dynamic>();
            parameters.Add("type", msgType);
            parameters.Add("isSticky", isSticky);

            var noticeMessage = FakeDataHelper.Create<NoticeMessage>(parameters);

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, Arg.Any<string>());

            // Assert
            Assert.AreEqual(expectedType, viewModel.MessageType);
        }

        [TestCase(Language.ChineseSimplified, "contentCS")]
        [TestCase(Language.ChineseTraditional, "contentCH")]
        [TestCase(Language.Thai, "contentTH")]
        [TestCase(Language.Japanese, "contentJP")]
        [TestCase(Language.Korean, "contentKO")]
        [TestCase(Language.Vietnamese, "contentVI")]
        [TestCase(Language.English, "contentEN")]
        public void Content_WhenCurrentLanguageContentIsNull_ReturnContentDefault(string lang, string contentPropName)
        {
            // Arrange
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>(contentPropName, string.Empty);

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, lang);

            // Assert
            Assert.AreEqual(noticeMessage.ContentEN, viewModel.Content);
        }

        [TestCase(Language.ChineseSimplified, "CS")]
        [TestCase(Language.ChineseTraditional, "CH")]
        [TestCase(Language.Thai, "TH")]
        [TestCase(Language.Japanese, "JP")]
        [TestCase(Language.Korean, "KO")]
        [TestCase(Language.Vietnamese, "EN")]
        [TestCase(Language.English, "EN")]
        public void Content_ByCurrentLanguage_ReturnContent(string lang, string expectedContent)
        {
            // Arrange
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("content" + expectedContent, expectedContent);

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, lang);

            // Assert
            Assert.AreEqual(expectedContent, viewModel.Content);
        }

        [TestCase(Language.ChineseSimplified, "CS")]
        [TestCase(Language.ChineseTraditional, "CH")]
        [TestCase(Language.Thai, "TH")]
        [TestCase(Language.Japanese, "JP")]
        [TestCase(Language.Korean, "KO")]
        [TestCase(Language.Vietnamese, "EN")]
        [TestCase(Language.English, "EN")]
        public void ShortContent_Always_ReturnData(string lang, string expectedShortContent)
        {
            // Arrange
            var noticeMessage = FakeDataHelper.Create<NoticeMessage>("content" + expectedShortContent, expectedShortContent);

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, lang);

            // Assert
            Assert.AreEqual(expectedShortContent + "...", viewModel.ShortContent);
        }
    }
}