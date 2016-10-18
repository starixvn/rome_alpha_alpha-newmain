namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Main.Constants;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Requests;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    [TestFixture]
    public class LanguageHandlerTests
    {
        private LanguageHandler languageHandler;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            languageHandler = new LanguageHandler();
            userSession = Substitute.For<IUserSession>();
        }

        [Test]
        public void HandleChangeLanguageCommand_NullCommand_ThrowArgumentNullException()
        {
            // Arrange
            ChangeLanguageCommand nullCommand = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => languageHandler.Handle(nullCommand)));
        }

        [Test]
        public void HandleChangeLanguageCommand_NotNullCommand_CallChangeLanguage()
        {
            // Arrange
            var command = Substitute.For<ChangeLanguageCommand>(userSession, Arg.Any<string>());

            string argumentUsed = null;
            command.ChangeLanguage(Arg.Do<string>(x => argumentUsed = x));

            // Action
            var viewModel = languageHandler.Handle(command);

            // Assert
            command.Received().ChangeLanguage(Arg.Any<string>());
        }

        [TestCase(null, Language.Default)]
        [TestCase("", Language.Default)]
        [TestCase(" ", Language.Default)]
        [TestCase("af-ZA ", Language.Default)]
        [TestCase("wrong-code ", Language.Default)]
        [TestCase(Language.Default, Language.Default)]
        [TestCase(Language.English, Language.English)]
        [TestCase(Language.ChineseTraditional, Language.ChineseTraditional)]
        [TestCase(Language.ChineseSimplified, Language.ChineseSimplified)]
        [TestCase(Language.Japanese, Language.Japanese)]
        [TestCase(Language.Thai, Language.Thai)]
        [TestCase(Language.Korean, Language.Korean)]
        [TestCase(Language.Vietnamese, Language.Vietnamese)]
        public void HandleChangeLanguageCommand_NotNullCommand_CallChangeLanguageWithCorrectLanguageCode(
            string languageCode,
            string expectedReceivedLanguageCode)
        {
            // Arrange
            string argumentUsed = null;
            var command = Substitute.For<ChangeLanguageCommand>(userSession, languageCode);
            command.ChangeLanguage(Arg.Do<string>(x => argumentUsed = x));

            // Action
            var viewModel = languageHandler.Handle(command);

            // Assert
            Assert.AreEqual(expectedReceivedLanguageCode, argumentUsed);
        }

        [TestCase(null, Language.Default)]
        [TestCase("", Language.Default)]
        [TestCase(" ", Language.Default)]
        [TestCase("af-ZA ", Language.Default)]
        [TestCase("wrong-code ", Language.Default)]
        [TestCase(Language.Default, Language.Default)]
        [TestCase(Language.English, Language.English)]
        [TestCase(Language.ChineseTraditional, Language.ChineseTraditional)]
        [TestCase(Language.ChineseSimplified, Language.ChineseSimplified)]
        [TestCase(Language.Japanese, Language.Japanese)]
        [TestCase(Language.Thai, Language.Thai)]
        [TestCase(Language.Korean, Language.Korean)]
        [TestCase(Language.Vietnamese, Language.Vietnamese)]
        public void HandleChangeLanguageCommand_NotNullCommand_ReturnCorrectLanguageCode(
            string languageCode,
            string expectedLanguageCode)
        {
            // Arrange
            var command = new ChangeLanguageCommand(userSession, languageCode);

            // Action
            var viewModel = languageHandler.Handle(command);

            // Assert
            Assert.AreEqual(expectedLanguageCode, viewModel);
        }

        [Test]
        public void HandleAllLanguagesQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            AllLanguagesQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => languageHandler.Handle(nullQuery)));
        }

        [TestCase(null, Language.Default)]
        [TestCase("", Language.Default)]
        [TestCase(" ", Language.Default)]
        [TestCase("af-ZA ", Language.Default)]
        [TestCase("wrong-code ", Language.Default)]
        [TestCase(Language.Default, Language.Default)]
        [TestCase(Language.English, Language.English)]
        [TestCase(Language.ChineseTraditional, Language.ChineseTraditional)]
        [TestCase(Language.ChineseSimplified, Language.ChineseSimplified)]
        [TestCase(Language.Japanese, Language.Japanese)]
        [TestCase(Language.Thai, Language.Thai)]
        [TestCase(Language.Korean, Language.Korean)]
        [TestCase(Language.Vietnamese, Language.Vietnamese)]
        public void HandleAllLanguagesQuery_NotNullCommand_ReturnViewModelWithCorrectDataAndLanguageCode(
            string languageCode,
            string expectedLanguageCode)
        {
            // Arrange
            var query = new AllLanguagesQuery(languageCode);

            var expectedViewModel = new LanguageViewModel(Language.CodeNames, expectedLanguageCode);

            // Action
            var viewModel = languageHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedViewModel, viewModel);
        }
    }
}
