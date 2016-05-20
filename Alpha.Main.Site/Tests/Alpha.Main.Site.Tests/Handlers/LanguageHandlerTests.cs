namespace Alpha.Main.Site.Tests.Handlers
{
    using Main.Constants;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Requests;
    using Site.Handlers;
    using Sunplus.Agent.Model.Entities;
    using System;
    using ViewModels;

    [TestFixture]
    public class LanguageHandlerTests
    {
        [Test]
        public void HandleChangeLanguageCommand_NullCommand_ThrowArgumentNullException()
        {
            // Arrange
            ChangeLanguageCommand command = null;
            var languageHandler = new LanguageHandler();

            // Action & Assert
            Assert.Throws<NullReferenceException>(new TestDelegate(() => languageHandler.Handle(command)));
        }

        [Test]
        public void HandleChangeLanguageCommand_NotNullCommand_CallChangeLanguage()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            var command = Substitute.For<ChangeLanguageCommand>(userSession, Arg.Any<string>());

            string argumentUsed = null;
            command.ChangeLanguage(Arg.Do<string>(x => argumentUsed = x));

            var languageHandler = new LanguageHandler();

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
            var userSession = Substitute.For<IUserSession>();
            string argumentUsed = null;
            var command = Substitute.For<ChangeLanguageCommand>(userSession, languageCode);
            command.ChangeLanguage(Arg.Do<string>(x => argumentUsed = x));

            var languageHandler = new LanguageHandler();

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
            var userSession = Substitute.For<IUserSession>();
            var command = Substitute.For<ChangeLanguageCommand>(userSession, languageCode);
            var languageHandler = new LanguageHandler();

            // Action
            var viewModel = languageHandler.Handle(command);

            // Assert
            Assert.AreEqual(expectedLanguageCode, viewModel);
        }

        [Test]
        public void HandleAllLanguagesQuery_NullCommand_ThrowArgumentNullException()
        {
            // Arrange
            AllLanguagesQuery query = null;
            var languageHandler = new LanguageHandler();

            // Action & Assert
            Assert.Throws<NullReferenceException>(new TestDelegate(() => languageHandler.Handle(query)));
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
        [TestCase(Language.Vietnamese, Language.Korean)]
        public void HandleAllLanguagesQuery_NotNullCommand_ReturnViewModelWithCorrectDataAndLanguageCode(
            string languageCode,
            string expectedLanguageCode)
        {
            // Arrange
            var query = new AllLanguagesQuery(languageCode);
            var languageHandler = new LanguageHandler();

            var expectedViewModel = new LanguageViewModel(Language.CodeNames, expectedLanguageCode);

            // Action
            var viewModel = languageHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedViewModel, viewModel);
        }
    }
}
