namespace Alpha.Main.Tests.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using Helps;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserMessageServiceImplTests
    {
        private UserMessageServiceImpl userMessageServiceImpl;

        [SetUp]
        public void SetUp()
        {
            userMessageServiceImpl = new UserMessageServiceImpl();
        }

        [Test]
        public void GetHotMessages_HotMessagesFileDoesNotExist_ThrowFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => userMessageServiceImpl.GetHotMessages(@"D:\demo.xml", 0, string.Empty));
        }

        [Test]
        public void GetHotMessages_HotMessagesFileDoesNotContainAnyData_ReturnEmptyEnumeration()
        {
            //// Arrange
            var hotMessageFilePath = FileHelper.GetFilePath(@"\TestData\EmptyToolTipMessage.xml");

            //// Act
            var actualHotMessages = userMessageServiceImpl.GetHotMessages(hotMessageFilePath, 0, string.Empty);

            //// Assert
            Assert.IsEmpty(actualHotMessages);
        }

        [TestCase("en-US", "en-US")]
        [TestCase("zh-CN", "zh-CN")]
        [TestCase("zh-TW", "zh-TW")]
        [TestCase("ja-JP", "ja-JP")]
        [TestCase("ko-KR", "ko-KR")]
        [TestCase("ko-KR", "ko-KR")]
        [TestCase("th-TH", "th-TH")]
        [TestCase("11111", "en-US")]
        public void GetHotMessages_HotMessageContainsData_ReturnHotMessage(string language, string expectedMessage)
        {
            //// Arrange
            var hotMessageFilePath = FileHelper.GetFilePath(@"\TestData\TooltipMessage.xml");

            //// Act
            var actualHotMessages = userMessageServiceImpl.GetHotMessages(hotMessageFilePath, 0, language);

            //// Assert
            Assert.AreEqual(expectedMessage, actualHotMessages.ElementAt(0));
        }

        [Test]
        public void GetHotMessage_LanguageIsVietNamese_ReturnEmpty()
        {
            //// Arrange
            var hotMessageFilePath = FileHelper.GetFilePath(@"\TestData\TooltipMessage.xml");

            //// Act
            var actualHotMessages = userMessageServiceImpl.GetHotMessages(hotMessageFilePath, 0, "vi-VN");

            //// Assert
            Assert.IsEmpty(actualHotMessages);
        }

        [Test]
        public void GetHotMessage_HotMessageIsExpired_ReturnEmpty()
        {
            //// Arrange
            var hotMessageFilePath = FileHelper.GetFilePath(@"\TestData\ExpiredMessage.xml");

            //// Act
            var actualHotMessages = userMessageServiceImpl.GetHotMessages(hotMessageFilePath, 0, "en-US");

            //// Assert
            Assert.IsEmpty(actualHotMessages);
        }

        [TestCase(2, "en-US-2")]
        [TestCase(3, "en-US-3")]
        [TestCase(4, "en-US-4")]
        public void GetHotMessage_UserWasOfferedHotMessage_ReturnHotMessage(byte roleId, string expectedMessage)
        {
            //// Arrange
            var hotMessageFilePath = FileHelper.GetFilePath(@"\TestData\OfferMessageByRoleId.xml");

            //// Act
            var actualHotMessages = userMessageServiceImpl.GetHotMessages(hotMessageFilePath, roleId, "en-US");

            //// Assert
            Assert.AreEqual(expectedMessage, actualHotMessages.ElementAt(0));
        }

        [TestCase(null)]
        [TestCase("  ")]
        [TestCase("")]
        public void GetHotMessages_LangugeIsNullOrWhiteSpace_ThrowArgumentNullException(string language)
        {
            //// Arrange
            var hotMessageFilePath = FileHelper.GetFilePath(@"\TestData\OfferMessageByRoleId.xml");

            //// Act & Assert
            userMessageServiceImpl
                .When(x => x.GetHotMessages(hotMessageFilePath, 2, language))
                .Do(x =>
                {
                    throw new ArgumentNullException(nameof(language), "Language cannot be null");
                });
        }
    }
}