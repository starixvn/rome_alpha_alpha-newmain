#pragma warning disable 618, 612

namespace Alpha.Main.Site.Tests.Requests
{
    using Main.Constants;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Requests;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class ChangeLanguageCommandTests
    {
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
        }

        [TestCase(Language.Default, Language.Default)]
        [TestCase(Language.English, Language.English)]
        [TestCase(Language.ChineseTraditional, Language.ChineseTraditional)]
        [TestCase(Language.ChineseSimplified, Language.ChineseSimplified)]
        [TestCase(Language.Japanese, Language.Japanese)]
        [TestCase(Language.Thai, Language.Thai)]
        [TestCase(Language.Korean, Language.Korean)]
        [TestCase(Language.Vietnamese, Language.Vietnamese)]
        public void HandleChangeLanguageCommand_WithLanguageCode_UpdateCorrectLanguageCode(
            string languageCode,
            string expectedLanguageCode)
        {
            // Arrange
            var command = new ChangeLanguageCommand(userSession, languageCode, false, false);

            // Act
            command.ChangeLanguage(languageCode);

            // Assert
            Assert.AreEqual(expectedLanguageCode, userSession.Language);
        }
    }
}