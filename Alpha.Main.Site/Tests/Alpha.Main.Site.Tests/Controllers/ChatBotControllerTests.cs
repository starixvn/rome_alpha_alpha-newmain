namespace Alpha.Main.Site.Tests.Controllers
{
    using Configurations;
    using MediatR;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Requests.ChatBot;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class ChatBotControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private ChatBotController chatbotController;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            chatbotController = new ChatBotController(mediator, userSession);
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
        public void AddAccount_Always_CallsMediatorSendAddAccountCommand()
        {
            // Arrange
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
    }
}