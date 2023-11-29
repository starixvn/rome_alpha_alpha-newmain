namespace Alpha.Main.Site.Tests.Handlers
{
    using System.Threading.Tasks;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Handlers;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using AlphaPlus.User.Models;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using TelegramBot.Client.Models;
    using TelegramBot.Client.Services;

    [TestFixture]
    [Category("TelegramChatBot")]
    public class TelegramChatBotHandlerTests
    {
        private const int UnlinkedErrorCode = 102;

        private CompareLogic compareLogic;
        private IUserSession userSession;
        private ISettingService settingService;
        private IBotService botService;
        private TelegramChatBotHandler handler;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(1);
            userSession.RoleId.Returns((byte)4);
            userSession.UserLoginIp.Returns("127.0.0.1");

            settingService = Substitute.For<ISettingService>();
            botService = Substitute.For<IBotService>();

            handler = new TelegramChatBotHandler(settingService, botService);
        }

        [Test]
        public async Task HandleSetUpChatBotQuery_Always_CallsGetCustBotMessengerAsync()
        {
            // Arrange
            userSession.IsSubAcc.Returns(true);
            var message = new SetUpChatBotQuery(userSession);

            // Act
            await handler.Handle(message);

            // Assert
            await settingService.Received().GetCustBotMessengerAsync(
                Arg.Is(message.CustId),
                Arg.Is(message.SubAccId),
                Arg.Is(Language.MapNewChineseLanguageCode(message.Language)));
        }

        [Test]
        public async Task HandleVerifyOTPCommand_Always_CallsLinkAsync()
        {
            // Arrange
            var botEntity = new BotEntity
            {
                BotId = 1
            };
            botService.GetBySystem(Arg.Any<string>()).Returns(botEntity);
            userSession.IsSubAcc.Returns(false);
            var message = new VerifyOTPCommand(userSession);

            // Act
            await handler.Handle(message);

            // Assert
            await settingService.Received().LinkAsync(
                Arg.Is(botEntity.BotId),
                Arg.Is(message.CustId),
                Arg.Is(message.SubAccId),
                Arg.Is(message.OTP),
                Arg.Is(message.Username),
                Arg.Is(message.IP));
        }

        [Test]
        public async Task HandleResendOTPCommand_Always_CallsResendOtpAsync()
        {
            // Arrange
            var message = new ResendOTPCommand(userSession);

            // Act
            await handler.Handle(message);

            // Assert
            await settingService.Received().ResendOtpAsync(Arg.Is(message.CustId), Arg.Is(message.SubAccId));
        }

        [Test]
        public async Task HandleUnsubscribeCommand_Always_CallsUnlinkAsync()
        {
            // Arrange
            var message = new UnsubscribeCommand(userSession);

            // Act
            await handler.Handle(message);

            // Assert
            await settingService.Received().UnlinkAsync(
                Arg.Is(message.CustId),
                Arg.Is(message.SubAccId),
                Arg.Is(message.IP));
        }

        [Test]
        public async Task HandleUpdateFunctionCommand_IsVerify_CallsUpdateUserFunction()
        {
            // Arrange
            var botEntity = new BotEntity
            {
                BotId = 1
            };
            botService.GetBySystem(Arg.Any<string>()).Returns(botEntity);
            var custBotMessenger = new CustBotMessenger
            {
                IsVerifyOTP = true,
                Enabled = true
            };
            settingService.GetCustBotMessengerAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>())
                .Returns(Task.FromResult(custBotMessenger));
            var message = new UpdateFunctionCommand(userSession);

            // Act
            await handler.Handle(message);

            // Assert
            await settingService.Received().UpdateUserFunction(
                Arg.Is(custBotMessenger.UserId),
                Arg.Is(botEntity.BotId),
                Arg.Is<Function>(
                    f => f.FunctionId == message.FunctionId
                    && f.FunctionName == message.FunctionName
                    && f.Enabled == message.Enabled
                    && f.Type == message.Type),
                Arg.Is(message.CustId),
                Arg.Is(message.IP));
        }

        [Test]
        public async Task HandleUpdateFunctionCommand_IsNotVerify_ReturnsUnlinkedErrorResult()
        {
            // Arrange
            var botEntity = new BotEntity
            {
                BotId = 1
            };
            botService.GetBySystem(Arg.Any<string>()).Returns(botEntity);
            var custBotMessenger = new CustBotMessenger
            {
                IsVerifyOTP = false,
                Enabled = false
            };
            settingService.GetCustBotMessengerAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>())
                .Returns(Task.FromResult(custBotMessenger));
            var message = new UpdateFunctionCommand(userSession);

            // Act
            var actualResult = await handler.Handle(message);

            // Assert
            Assert.IsTrue(compareLogic.Compare(new Result(UnlinkedErrorCode), actualResult).AreEqual);
        }

        [Test]
        public async Task HandleUpdateLanguageCommand_IsVerify_CallsUpdateLanguage()
        {
            // Arrange
            var botEntity = new BotEntity
            {
                BotId = 1
            };
            botService.GetBySystem(Arg.Any<string>()).Returns(botEntity);
            var custBotMessenger = new CustBotMessenger
            {
                IsVerifyOTP = true,
                Enabled = true
            };
            settingService.GetCustBotMessengerAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>())
                .Returns(Task.FromResult(custBotMessenger));
            var message = new UpdateLanguageCommand(userSession);

            // Act
            await handler.Handle(message);

            // Assert
            await settingService.Received().UpdateLanguage(
                Arg.Is(custBotMessenger.UserId),
                Arg.Is(botEntity.BotId),
                Arg.Is(Language.MapNewChineseLanguageCode(message.Language)),
                Arg.Is(message.CustId),
                Arg.Is(message.IP));
        }
    }
}