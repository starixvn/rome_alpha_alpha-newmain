#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Requests;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class LanguageHandlerTests
    {
        private LanguageHandler languageHandler;
        private IUserSession userSession;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            languageHandler = new LanguageHandler();
            userSession = Substitute.For<IUserSession>();
            compareLogic = new CompareLogic();
        }

        [Test]
        public void HandleChangeLanguageCommand_NullCommand_ThrowArgumentNullException()
        {
            // Arrange
            ChangeLanguageCommand nullCommand = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => languageHandler.Handle(nullCommand)));
        }

        [Test]
        public void HandleChangeLanguageCommand_NotNullCommand_CallChangeLanguage()
        {
            // Arrange
            var command = Substitute.For<ChangeLanguageCommand>(userSession, Arg.Any<string>(), Arg.Any<bool>());

            string argumentUsed = null;
            command.ChangeLanguage(Arg.Do<string>(x => argumentUsed = x));

            // Act
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
            var command = Substitute.For<ChangeLanguageCommand>(userSession, languageCode, false);
            command.ChangeLanguage(Arg.Do<string>(x => argumentUsed = x));

            // Act
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
            var command = new ChangeLanguageCommand(userSession, languageCode, false);

            // Act
            var viewModel = languageHandler.Handle(command);

            // Assert
            Assert.AreEqual(expectedLanguageCode, viewModel);
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
        [TestCase(Language.Vietnamese, Language.English)]
        public void HandleChangeLanguageCommand_DisabledVietnamese_SetUserSessionLanguage(
            string languageCode,
            string expectedLanguageCode)
        {
            // Arrange
            var command = new ChangeLanguageCommand(userSession, languageCode, true);

            // Act
            languageHandler.Handle(command);

            // Assert
            Assert.AreEqual(expectedLanguageCode, userSession.Language);
        }

        [Test]
        public void HandleAllLanguagesQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            AllLanguagesQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => languageHandler.Handle(nullQuery)));
        }

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
            var query = new AllLanguagesQuery(languageCode, false);

            var expectedViewModel = new LanguageViewModel(Language.CodeNames, expectedLanguageCode);

            // Act
            var viewModel = languageHandler.Handle(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, viewModel).AreEqual);
        }

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
        [TestCase(Language.Vietnamese, Language.English)]
        public void HandleAllLanguagesQuery_DisabledVietnamese_ReturnViewModelWithCorrectDataAndLanguageCode(
            string languageCode,
            string expectedLanguageCode)
        {
            // Arrange
            var query = new AllLanguagesQuery(languageCode, true);

            var expectedViewModel = new LanguageViewModel(Language.CodeNamesWithoutVietNamese, expectedLanguageCode);

            // Act
            languageHandler.Handle(query);

            // Assert
            Assert.AreEqual(Language.CodeNamesWithoutVietNamese, expectedViewModel.Data);
        }
    }
}
