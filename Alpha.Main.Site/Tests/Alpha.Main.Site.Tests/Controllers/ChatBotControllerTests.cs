namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Chatbot.Client.Models;
    using Configurations;
    using KellermanSoftware.CompareNetObjects;
    using Main.Models;
    using MediatR;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Requests.ChatBot;
    using Site.ViewModels.ChatBot;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class ChatBotControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private ChatBotController chatbotController;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            chatbotController = new ChatBotController(mediator, userSession);
            compareLogic = new CompareLogic();
        }

        [Test]
        public void Index_Always_CallsMediatorSendSetupChatBotQuery()
        {
            // Arrange
            var query = new SetUpChatBotQuery(userSession);

            // Act
            chatbotController.Index().ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<SetUpChatBotQuery>(
                    q => q.CustId == query.CustId
                        && q.SubAccountId == query.SubAccountId
                        && q.Language == query.Language));
        }

        [Test]
        public void Index_AlreadySetup_ReturnsCorrectView()
        {
            // Arrange
            var user = new Chatbot.Client.Models.User
            {
                CustId = 1,
                MessengerAccounts = new List<Messenger>
                {
                    new Messenger {
                        Type= MessengerType.Letstalk,
                        Enabled=true
                    }
                }
            };

            var viewModel = new ChatBotViewModel(user);

            mediator.SendAsync(Arg.Any<SetUpChatBotQuery>()).Returns(Task.FromResult(viewModel));

            // Act
            var result = chatbotController.Index().GetAwaiter().GetResult() as ViewResult;

            // Assert
            Assert.True(result.ViewName.Equals("ChatBotSetting")
                && compareLogic.Compare(new ChatBotSettingViewModel(user), result.Model).AreEqual);
        }

        [Test]
        public void Index_IsNotAlreadySetup_ReturnsCorrectView()
        {
            // Arrange
            var user = new Chatbot.Client.Models.User
            {
                CustId = 1,
                MessengerAccounts = new List<Messenger>()
            };

            var viewModel = new ChatBotViewModel(user);

            mediator.SendAsync(Arg.Any<SetUpChatBotQuery>()).Returns(Task.FromResult(viewModel));

            // Act
            var result = chatbotController.Index().GetAwaiter().GetResult() as ViewResult;

            // Assert
            Assert.True(result.ViewName.Equals("RegisterAccount")
                && compareLogic.Compare(new RegisterAccountViewModel(user), result.Model).AreEqual);
        }

        [Test]
        public void AddAccount_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var expectedData = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var result = chatbotController.AddAccount(new AddAccountCommand(userSession))
                .GetAwaiter().GetResult() as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, result.Data).AreEqual);
        }

        [Test]
        public void AddAccount_IsNotInternal_CallsMediatorSendAddAccountCommand()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new AddAccountCommand(userSession);
            command.MessengerAccount = "MessengerAccount";

            // Act
            chatbotController.AddAccount(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<AddAccountCommand>(
                    c => c.CustId == command.CustId
                        && c.SubAccountId == command.SubAccountId
                        && c.Language == command.Language
                        && c.MessengerAccount == command.MessengerAccount));
        }

        [Test]
        public void AddAccount_ResultIsOk_ReturnsCorrectViewModel()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new AddAccountCommand(userSession);
            command.MessengerAccount = "MessengerAccount";

            // Act
            chatbotController.AddAccount(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<AddAccountCommand>(
                    c => c.CustId == command.CustId
                        && c.SubAccountId == command.SubAccountId
                        && c.Language == command.Language
                        && c.MessengerAccount == command.MessengerAccount));
        }

        [Test]
        public void Unsubscribe_Always_CallMediatorSendUnsubscribeCommand()
        {
            // Arrange
            var command = new UnsubscribeCommand();

            // Act
            chatbotController.Unsubscribe(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<UnsubscribeCommand>(
                    c => c.MessengerType == command.MessengerType
                        && c.MessengerUserName == command.MessengerUserName
                        && c.UserId == command.UserId));
        }

        [Test]
        public void ResendVerificationCode_Always_CallMediatorSendResendVerificationCodeCommand()
        {
            // Arrange
            var command = new ResendVerificationCodeCommand();

            // Act
            chatbotController.ResendVerificationCode(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<ResendVerificationCodeCommand>(
                    c => c.UserId == command.UserId
                        && c.MessengerUsername == command.MessengerUsername
                        && c.UserId == command.UserId));
        }

        [TestCase("en-us")]
        [TestCase("ja-jp")]
        [TestCase("ko-kr")]
        [TestCase("th-th")]
        public void Language_Always_CallsMediatorSendAllLanguagesQuery(string langCode)
        {
            // Act
            var query = new AllLanguagesQuery(langCode, AppSettings.DisabledVietnamese);
            chatbotController.Language(langCode);

            // Assert
            mediator.Received().Send(
                Arg.Is<AllLanguagesQuery>(
                    param => param.LanguageCode == query.LanguageCode
                    && param.DisabledVietnamese == query.DisabledVietnamese));
        }

        [Test]
        public void UpdateFunction_Always_CallMediatorSendUpdateFunctionCommand()
        {
            // Arrange
            var command = new UpdateFunctionCommand();

            // Act
            chatbotController.UpdateFunction(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<UpdateFunctionCommand>(
                    c => c.UserId == command.UserId
                        && c.FunctionId == command.FunctionId
                        && c.Command == command.Command
                        && c.IsEnable == command.IsEnable));
        }

        [Test]
        public void ChangeLanguage_Always_CallMediatorSendUpdateLanguageCommand()
        {
            // Arrange
            var command = new UpdateLanguageCommand();

            // Act
            chatbotController.ChangeLanguage(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<UpdateLanguageCommand>(
                    c => c.UserId == command.UserId
                        && c.Language == command.Language));
        }

        [Test]
        public void Captcha_Always_ReturnsPartialView()
        {
            // Act
            var result = chatbotController.Captcha();

            // Assert
            Assert.IsTrue(result is PartialViewResult);
        }

        [Test]
        public void ReportExample_Always_returnCorrectView()
        {
            // Arrange
            var language = "en-US";

            // Act
            var result = chatbotController.ReportExample(language) as ViewResult;

            // Assert
            Assert.IsTrue(result.ViewName.Equals("ReportExample") && result.Model == (object)language);
        }

        [Test]
        public void NotificationExample_Always_returnCorrectView()
        {
            // Arrange
            var language = "en-US";

            // Act
            var result = chatbotController.NotificationExample(language) as ViewResult;

            // Assert
            Assert.IsTrue(result.ViewName.Equals("NotificationExample") && result.Model == (object)language);
        }
    }
}