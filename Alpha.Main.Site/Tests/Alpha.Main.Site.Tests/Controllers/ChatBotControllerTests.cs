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
    [Category("Chabot")]
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
            command.LetstalkId = "MessengerAccount";

            // Act
            chatbotController.AddAccount(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<AddAccountCommand>(
                    c => c.CustId == command.CustId
                        && c.SubAccountId == command.SubAccountId
                        && c.Language == command.Language
                        && c.LetstalkId == command.LetstalkId));
        }

        [Test]
        public void AddAccount_ResultIsOk_ReturnsCorrectViewModel()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new AddAccountCommand(userSession);
            command.LetstalkId = "MessengerAccount";
            var result = new AddingUserResult { Code = 0 };
            mediator.SendAsync(Arg.Any<AddAccountCommand>()).Returns(Task.FromResult(result));
            var viewModel = new VerifyAccountViewModel();

            // Act
            var viewResult = chatbotController.AddAccount(command).GetAwaiter().GetResult() as PartialViewResult;

            // Assert
            Assert.IsTrue(viewResult.ViewName.Equals("VerifyAccount")
                && compareLogic.Compare(viewModel, viewResult.Model).AreEqual);
        }

        [Test]
        public void Unsubscribe_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var expectedData = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var result = chatbotController.Unsubscribe(new UnsubscribeCommand())
                .GetAwaiter().GetResult() as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, result.Data).AreEqual);
        }

        [Test]
        public void Unsubscribe_IsNotInternal_CallMediatorSendUnsubscribeCommand()
        {
            // Arrange
            var command = new UnsubscribeCommand(userSession);

            // Act
            chatbotController.Unsubscribe().ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<UnsubscribeCommand>(
                    c => c.CustId == command.CustId
                        && c.IP == command.IP));
        }

        [Test]
        public void VerifyVerificationCode_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var expectedData = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var result = chatbotController.VerifyVerificationCode(new VerifyVerificationCodeCommand())
                .GetAwaiter().GetResult() as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, result.Data).AreEqual);
        }

        [Test]
        public void ResendVerificationCode_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var expectedData = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var result = chatbotController.ResendVerificationCode(new ResendVerificationCodeCommand())
                .GetAwaiter().GetResult() as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, result.Data).AreEqual);
        }

        [Test]
        public void ResendVerificationCode_IsNotInternal_CallMediatorSendResendVerificationCodeCommand()
        {
            // Arrange
            var command = new ResendVerificationCodeCommand();

            // Act
            chatbotController.ResendVerificationCode().ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<ResendVerificationCodeCommand>(
                    c => c.IntervalThresholdToSendRequestInSeconds == command.IntervalThresholdToSendRequestInSeconds));
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
        public void UpdateFunction_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var expectedData = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var result = chatbotController.UpdateFunction(new UpdateFunctionCommand())
                .GetAwaiter().GetResult() as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, result.Data).AreEqual);
        }

        [Test]
        public void UpdateFunction_IsNotInternal_CallMediatorSendUpdateFunctionCommand()
        {
            // Arrange
            var command = new UpdateFunctionCommand();

            // Act
            chatbotController.UpdateFunction(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<UpdateFunctionCommand>(
                    c => c.FunctionId == command.FunctionId
                        && c.Command == command.Command
                        && c.IsEnable == command.IsEnable));
        }

        [Test]
        public void ChangeLanguage_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var expectedData = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var result = chatbotController.ChangeLanguage(new UpdateLanguageCommand())
                .GetAwaiter().GetResult() as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, result.Data).AreEqual);
        }

        [Test]
        public void ChangeLanguage_IsNotInternal_CallMediatorSendUpdateLanguageCommand()
        {
            // Arrange
            var command = new UpdateLanguageCommand();

            // Act
            chatbotController.ChangeLanguage(command).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(
                Arg.Is<UpdateLanguageCommand>(c => c.Language == command.Language));
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