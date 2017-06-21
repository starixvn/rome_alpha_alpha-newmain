using Alpha.Main.Configurations;
using Alpha.Main.Site.Controllers;
using Alpha.Main.Site.Models.Requests;
using Alpha.Main.Site.Requests;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;
using System;

namespace Alpha.Main.Site.Tests.Controllers
{
    [TestFixture]
    public class LanguageControllerTests
    {
        private LanguageController languageController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            languageController = new LanguageController(mediator, userSession);
        }

        [Test]
        public void Constructor_UserSessionIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            userSession = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LanguageController(mediator, userSession));
        }

        [Test]
        public void List_Always_CallsMediatorSendAllLanguagesQuery()
        {
            // Act
            var query = new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese);
            languageController.List();

            // Assert
            mediator.Received().Send(
                Arg.Is<AllLanguagesQuery>(
                    param => param.LanguageCode == query.LanguageCode
                    && param.DisabledVietnamese == query.DisabledVietnamese));
        }

        [TestCase("en-us")]
        [TestCase("ja-jp")]
        [TestCase("ko-kr")]
        [TestCase("th-th")]
        public void Change_Always_CallsMediatorSendChangeLanguageCommand(string languageCode)
        {
            // Act
            var command = new ChangeLanguageCommand(userSession, languageCode, AppSettings.DisabledVietnamese);
            languageController.Change(languageCode);

            // Assert
            mediator.Received().Send(Arg.Is<ChangeLanguageCommand>(
                param => param.LanguageCode == command.LanguageCode));
        }
    }
}
