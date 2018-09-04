namespace Alpha.Main.Site.Tests.Handlers
{
    using System.Collections.Generic;
    using Chatbot.Client.Models;
    using Chatbot.Client.Services;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Requests.ChatBot;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    [TestFixture]
    public class ChatBotHandlerTests
    {
        private IUserSession userSession;
        private SettingService settingService;
        private ChatBotHandler chatbotHandler;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            settingService = Substitute.For<SettingService>();
            chatbotHandler = new ChatBotHandler(settingService, Substitute.For<SessionManager>());
        }

        [Test]
        public void HandleSetUpChatBotQuery_Always_CallsSettingServiceGetUser()
        {
            // Arrange
            var message = new SetUpChatBotQuery(userSession);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().GetUser(
                Arg.Is(message.CustId),
                Arg.Is(message.SubAccountId),
                Arg.Is(message.Language));
        }

        [Test]
        public void HandleAddAccountCommand_Always_CallsSettingServiceAddUser()
        {
            // Arrange
            var message = new AddAccountCommand(userSession);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().AddUser(
                Arg.Is(message.CustId),
                Arg.Is(message.CustLevel),
                Arg.Is(message.SubAccountId),
                Arg.Is(message.Language),
                Arg.Is(message.MessengerAccount));
        }

        [Test]
        public void HandleUnsubscribeCommand_Always_CallsSettingServiceUnsubscribe()
        {
            // Arrange
            var message = new UnsubscribeCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().Unsubscribe(
                Arg.Is(message.UserId),
                Arg.Is(message.MessengerType),
                Arg.Is(message.MessengerUserName));
        }

        [Test]
        public void HandleVerifyVerificationCodeCommand_Always_CallsSettingServiceVerifyVerificationCode()
        {
            // Arrange
            var message = new VerifyVerificationCodeCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().VerifyVerificationCode(
                Arg.Is(message.UserId),
                Arg.Is(message.VerificationCode),
                Arg.Is(message.MessengerAccount),
                Arg.Is(message.MessengerType));
        }

        [Test]
        public void HandleResendVerificationCodeCommand_Always_CallsSettingServiceGenerateVerificationCode()
        {
            // Arrange
            var message = new ResendVerificationCodeCommand(userSession);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().GenerateVerificationCode(
                Arg.Is(message.UserId),
                Arg.Is(message.MessengerUsername));
        }

        [Test]
        public void HandleUpdateFunctionCommand_Always_CallsSettingServiceUpdateFunctions()
        {
            // Arrange
            var message = new UpdateFunctionCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().UpdateFunctions(
                Arg.Is(message.UserId),
                Arg.Any<IEnumerable<Function>>());
        }

        [Test]
        public void HandleUpdateLanguageCommand_Always_CallsSettingServicUpdateLanguage()
        {
            // Arrange
            var message = new UpdateLanguageCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().UpdateLanguage(
                Arg.Is(message.UserId),
                Arg.Is(message.Language));
        }
    }
}