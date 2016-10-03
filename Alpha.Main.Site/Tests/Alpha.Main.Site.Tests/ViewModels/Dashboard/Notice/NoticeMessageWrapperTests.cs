namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using System;
    using Main.Constants;
    using Main.Models.Notice;
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
        public void Type_ByMessageTypeAndIsSticky_ReturnTypeText(byte msgType, bool isSticky, string expectedType)
        {
            // Arrange
            var noticeMessage = CreateNoticeMessagesData(msgType, isSticky);

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, Arg.Any<string>());

            // Assert
            Assert.AreEqual(expectedType, viewModel.Type);
        }

        [TestCase(Language.ChineseSimplified, "cs")]
        [TestCase(Language.ChineseTraditional, "ch")]
        [TestCase(Language.Thai, "th")]
        [TestCase(Language.Japanese, "jp")]
        [TestCase(Language.Korean, "ko")]
        [TestCase(Language.Vietnamese, "en")]
        [TestCase(Language.English, "en")]
        public void Content_ByCurrentLanguage_ReturnContent(string lang, string expectedContent)
        {
            // Arrange
            var noticeMessage = CreateNoticeMessagesData();

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, lang);

            // Assert
            Assert.AreEqual(expectedContent, viewModel.Content);
        }

        [TestCase(Language.ChineseSimplified, "cs")]
        [TestCase(Language.ChineseTraditional, "ch")]
        [TestCase(Language.Thai, "th")]
        [TestCase(Language.Japanese, "jp")]
        [TestCase(Language.Korean, "ko")]
        [TestCase(Language.Vietnamese, "en")]
        [TestCase(Language.English, "en")]
        public void ShortContent_Always_ReturnData(string lang, string expectedShortContent)
        {
            // Arrange
            var noticeMessage = CreateNoticeMessagesData();

            // Act
            var viewModel = new NoticeMessageWrapper(noticeMessage, lang);

            // Assert
            Assert.AreEqual(expectedShortContent + "...", viewModel.ShortContent);
        }

        private NoticeMessage CreateNoticeMessagesData(byte msgType = 1, bool isSticky = false)
        {
            var noticeMessage = new NoticeMessage(
                1,
                "en",
                "cs",
                "ch",
                "jp",
                "th",
                "lt",
                "ko",
                "es",
                DateTime.Today,
                msgType,
                isSticky,
                DateTime.Today,
                "1",
                true);

            return noticeMessage;
        }
    }
}