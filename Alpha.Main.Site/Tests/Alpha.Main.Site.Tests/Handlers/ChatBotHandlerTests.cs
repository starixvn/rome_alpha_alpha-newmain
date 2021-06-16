namespace Alpha.Main.Site.Tests.Handlers
{
    using Chatbot.Client.Models;
    using Chatbot.Client.Services;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Requests.ChatBot;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    [TestFixture]
    [Category("Chabot")]
    public class ChatBotHandlerTests
    {
        private const string ChatbotUserIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.UserId";
        private const string LetstalkIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.LetstalkId";
        private const string LetstalkAccountEnabledStatusKey = "Alpha.Main.Site.Handlers.ChatBotHandler.Status.Enabled";
        private const string LetstalkAccountIsVerifyingStatusKey = "Alpha.Main.Site.Handlers.ChatBotHandler.Status.IsVerifying";

        private IUserSession userSession;
        private SettingService settingService;
        private SessionManager sessionManager;
        private ChatBotHandler chatbotHandler;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(1);
            userSession.UserLoginIp.Returns("127.0.0.1");

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
        public void HandleAddAccountCommand_IsAllowLinkAccount_CallsSettingServiceAddUser()
        {
            // Arrange
            var message = new AddAccountCommand(userSession);
            var logParam = CreateLogParam(message.CustId, message.CustId, message.IP);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().AddUser(
                message.CustId,
                message.CustLevel,
                message.SubAccountId,
                message.Language,
                message.LetstalkId,
                MessengerType.Letstalk,
                Arg.Is<LogParam>(p => p.ActorId == logParam.ActorId
                                    && p.CustId == logParam.CustId
                                    && p.IP == logParam.IP));
        }

        [Test]
        public void HandleAddAccountCommand_AccountStatusIsVerifying_ReturnsErrorResult()
        {
            // Arrange
            var message = new AddAccountCommand(userSession);
            sessionManager.Get<bool>(LetstalkAccountIsVerifyingStatusKey).Returns(true);
            var expectedResult = new AddingUserResult { Code = 1 };

            // Act
            var actualResult = chatbotHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(expectedResult.Code, actualResult.Code);
        }

        [Test]
        public void HandleAddAccountCommand_AccountStatusIsEnabled_ReturnsErrorResult()
        {
            // Arrange
            var message = new AddAccountCommand(userSession);
            sessionManager.Get<bool>(LetstalkAccountEnabledStatusKey).Returns(true);
            var expectedResult = new AddingUserResult { Code = 1 };

            // Act
            var actualResult = chatbotHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(expectedResult.Code, actualResult.Code);
        }

        [Test]
        public void HandleUnsubscribeCommand_Always_CallsSettingServiceUnsubscribe()
        {
            // Arrange
            var userId = 1;
            var letstalkId = "LetstalkId";
            var message = new UnsubscribeCommand(userSession);
            var logParam = CreateLogParam(message.CustId, message.CustId, message.IP);

            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            sessionManager.Get<string>(LetstalkIdSessionKey).Returns(letstalkId);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().Unsubscribe(
                userId,
                MessengerType.Letstalk,
                letstalkId,
                Arg.Is<LogParam>(p => p.ActorId == logParam.ActorId
                                    && p.CustId == logParam.CustId
                                    && p.IP == logParam.IP));
        }

        [Test]
        public void HandleVerifyVerificationCodeCommand_AccountStatusIsVerifying_CallsSettingServiceVerifyVerificationCode()
        {
            // Arrange
            var userId = 1;
            var letstalkId = "LetstalkId";
            var message = new VerifyVerificationCodeCommand(userSession);
            var logParam = CreateLogParam(message.CustId, message.CustId, message.IP);

            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            sessionManager.Get<string>(LetstalkIdSessionKey).Returns(letstalkId);
            sessionManager.Get<bool>(LetstalkAccountIsVerifyingStatusKey).Returns(true);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().VerifyVerificationCode(
                userId,
                message.VerificationCode,
                letstalkId,
                MessengerType.Letstalk,
                Arg.Is<LogParam>(p => p.ActorId == logParam.ActorId
                                    && p.CustId == logParam.CustId
                                    && p.IP == logParam.IP));
        }

        [Test]
        public void HandleResendVerificationCodeCommand_AccountStatusIsVerifying_CallsSettingServiceGenerateVerificationCode()
        {
            // Arrange
            var userId = 1;
            var letstalkId = "LetstalkId";
            var message = new ResendVerificationCodeCommand();

            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            sessionManager.Get<string>(LetstalkIdSessionKey).Returns(letstalkId);
            sessionManager.Get<bool>(LetstalkAccountIsVerifyingStatusKey).Returns(true);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().GenerateVerificationCode(userId, letstalkId);
        }

        [Test]
        public void HandleUpdateFunctionCommand_Always_CallsSettingServiceUpdateFunctions()
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new UpdateFunctionCommand(userSession);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received();
        }

        [Test]
        public void HandleUpdateLanguageCommand_Always_CallsSettingServicUpdateLanguage()
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var message = new UpdateLanguageCommand(userSession) { Language = "en-US" };
            var logParam = CreateLogParam(message.CustId, message.CustId, message.IP);

            // Act
            chatbotHandler.Handle(message).ConfigureAwait(false);

            // Assert
            settingService.Received().UpdateLanguage(
                userId,
                message.Language,
                Arg.Is<LogParam>(p => p.ActorId == logParam.ActorId
                                    && p.CustId == logParam.CustId
                                    && p.IP == logParam.IP));
        }

        [Test]
        public void HandleUpdateSelfdestructionPeriodCommand_InvalidPeriod_ReturnsErrorResult()
        {
            // Arrange
            var command = new UpdateSelfdestructionPeriodCommand { Minutes = 13 };
            var expectedResult = new Result { Code = 1 };
            var compareLogic = new CompareLogic();

            // Act
            var result = chatbotHandler.Handle(command).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedResult, result).AreEqual);
        }

        [TestCase(SelfdestructionPeriod.FifteenDays)]
        [TestCase(SelfdestructionPeriod.FifteenMinutes)]
        [TestCase(SelfdestructionPeriod.OneDay)]
        [TestCase(SelfdestructionPeriod.OneHour)]
        [TestCase(SelfdestructionPeriod.SevenDays)]
        [TestCase(SelfdestructionPeriod.SixHours)]
        [TestCase(SelfdestructionPeriod.ThirtyDays)]
        [TestCase(SelfdestructionPeriod.ThirtyMinutes)]
        [TestCase(SelfdestructionPeriod.ThreeDays)]
        [TestCase(SelfdestructionPeriod.ThreeHours)]
        [TestCase(SelfdestructionPeriod.TwelveHours)]
        public void HandleUpdateSelfdestructionPeriodCommand_ValidPeriod_CallsUpdateSelfdestructionPeriod(int period)
        {
            // Arrange
            var userId = 1;
            sessionManager.Get<int>(ChatbotUserIdSessionKey).Returns(userId);
            var command = new UpdateSelfdestructionPeriodCommand(userSession) { Minutes = period };
            var logParam = CreateLogParam(command.CustId, command.CustId, command.IP);

            // Act
            chatbotHandler.Handle(command).ConfigureAwait(false);

            // Assert
            settingService.Received().UpdateSelfdestructionPeriod(
                userId,
                period,
                Arg.Is<LogParam>(p => p.ActorId == logParam.ActorId
                                    && p.CustId == logParam.CustId
                                    && p.IP == logParam.IP));
        }

        private static LogParam CreateLogParam(int actorId, int custId, string ip)
            => new LogParam
            {
                ActorId = actorId,
                CustId = custId,
                IP = ip
            };
    }
}