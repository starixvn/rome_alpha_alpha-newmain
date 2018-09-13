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
        private const string ChatbotUserIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.UserId";

        private IUserSession userSession;
        private SettingService settingService;
        private SessionManager sessionManager;
        private ChatBotHandler chatbotHandler;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            settingService = Substitute.For<SettingService>();
            sessionManager = Substitute.For<SessionManager>();

            chatbotHandler = new ChatBotHandler(settingService, sessionManager);
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
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new UnsubscribeCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().Unsubscribe(
                Arg.Is(userId),
                Arg.Is(message.MessengerType),
                Arg.Is(message.MessengerUserName));
        }

        [Test]
        public void HandleVerifyVerificationCodeCommand_Always_CallsSettingServiceVerifyVerificationCode()
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new VerifyVerificationCodeCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().VerifyVerificationCode(
                Arg.Is(userId),
                Arg.Is(message.VerificationCode),
                Arg.Is(message.MessengerAccount),
                Arg.Is(message.MessengerType));
        }

        [Test]
        public void HandleResendVerificationCodeCommand_Always_CallsSettingServiceGenerateVerificationCode()
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new ResendVerificationCodeCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().GenerateVerificationCode(
                Arg.Is(userId),
                Arg.Is(message.MessengerUsername));
        }

        [Test]
        public void HandleUpdateFunctionCommand_Always_CallsSettingServiceUpdateFunctions()
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new UpdateFunctionCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().UpdateFunctions(
                Arg.Is(userId),
                Arg.Any<IEnumerable<Function>>());
        }

        [Test]
        public void HandleUpdateLanguageCommand_Always_CallsSettingServicUpdateLanguage()
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new UpdateLanguageCommand();

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().UpdateLanguage(
                Arg.Is(userId),
                Arg.Is(message.Language));
        }
    }
}