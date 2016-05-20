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
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("af-ZA ", "")]
        [TestCase("wrong-code ", "")]
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
            var userSession = Substitute.For<IUserSession>();
            var command = new ChangeLanguageCommand(userSession, languageCode);

            // Action
            command.ChangeLanguage(languageCode);

            // Assert
            Assert.AreEqual(expectedLanguageCode, userSession.Language);
        }
    }
}
