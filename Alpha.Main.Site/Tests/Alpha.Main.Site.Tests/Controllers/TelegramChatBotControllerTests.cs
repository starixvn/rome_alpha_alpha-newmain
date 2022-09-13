namespace Alpha.Main.Site.Tests.Controllers
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using TelegramBot.Client.Models;

    [TestFixture]
    [Category("TelegramChatBot")]
    public class TelegramChatBotControllerTests
    {
        private TelegramChatBotController controller;
        private IMediator mediator;
        private IUserSession userSession;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            controller = new TelegramChatBotController(mediator, userSession);
            compareLogic = new CompareLogic();
        }

        [Test]
        public async Task Setup_IsLinked_ReturnsChatBotSettingViewModel()
        {
            // Arrange
            userSession.RoleId.Returns((byte)4);
            userSession.IsSubAcc.Returns(true);
            var query = new SetUpChatBotQuery(userSession);

            var custBotMessenger = new CustBotMessenger
            {
                IsVerifyOTP = true,
                Enabled = true,
                Functions = Enumerable.Empty<Function>()
            };
            var chatBotViewModel = new ChatBotViewModel(custBotMessenger);
            mediator.SendAsync(Arg.Any<SetUpChatBotQuery>()).Returns(Task.FromResult(chatBotViewModel));

            var expectedViewModel = new ChatBotSettingViewModel(chatBotViewModel, query);

            // Act
            var actualResult = await controller.Setup(query) as ViewResult;

            // Assert
            var actualViewModel = (ChatBotSettingViewModel)actualResult.Model;
            Assert.True(actualResult.ViewName.Equals("ChatBotSetting"));
            Assert.True(compareLogic.Compare(expectedViewModel.ChatBotViewModel, actualViewModel.ChatBotViewModel).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.UserId, actualViewModel.UserId).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.ChatId, actualViewModel.ChatId).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.ChatBotName, actualViewModel.ChatBotName).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.ReportFunctions, actualViewModel.ReportFunctions).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.NotificationFunctions, actualViewModel.NotificationFunctions).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.Language, actualViewModel.Language).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.CustLevel, actualViewModel.CustLevel).AreEqual);
            Assert.True(compareLogic.Compare(expectedViewModel.IsShowLetstalkChatBot, actualViewModel.IsShowLetstalkChatBot).AreEqual);
        }

        [Test]
        public async Task Setup_IsNotLinked_ReturnsRegisterAccountViewModel()
        {
            // Arrange
            userSession.RoleId.Returns((byte)4);
            userSession.IsSubAcc.Returns(true);
            var query = new SetUpChatBotQuery(userSession);

            var custBotMessenger = new CustBotMessenger
            {
                IsVerifyOTP = false,
                Enabled = false,
                Functions = Enumerable.Empty<Function>()
            };
            var chatBotViewModel = new ChatBotViewModel(custBotMessenger);
            mediator.SendAsync(Arg.Any<SetUpChatBotQuery>()).Returns(Task.FromResult(chatBotViewModel));

            var expectedViewModel = new RegisterAccountViewModel(chatBotViewModel, query);

            // Act
            var actualResult = await controller.Setup(query) as ViewResult;

            // Assert
            Assert.True(actualResult.ViewName.Equals("RegisterAccount"));
            Assert.True(compareLogic.Compare(expectedViewModel, actualResult.Model).AreEqual);
        }

        [Test]
        public async Task Unsubscribe_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new UnsubscribeCommand(userSession);
            var expectedResult = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var actualResult = await controller.Unsubscribe(command) as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedResult, actualResult.Data).AreEqual);
        }

        [Test]
        public async Task Unsubscribe_IsNotInternal_CallsMediatorSendUnsubscribeCommand()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new UnsubscribeCommand(userSession);

            // Act
            await controller.Unsubscribe(command);

            // Assert
            await mediator.Received().SendAsync(
                Arg.Is<UnsubscribeCommand>(
                    c => c.CustId == command.CustId
                    && c.SubAccId == command.SubAccId
                    && c.IP == command.IP));
        }

        [Test]
        public async Task GenerateQRCode_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new GenerateQRCodeCommand(userSession);
            var expectedResult = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var actualResult = await controller.GenerateQRCode(command) as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedResult, actualResult.Data).AreEqual);
        }

        [Test]
        public async Task GenerateQRCode_IsNotInternal_CallsMediatorGenerateQRCodeCommand()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new GenerateQRCodeCommand(userSession);

            // Act
            await controller.GenerateQRCode(command);

            // Assert
            await mediator.Received().SendAsync(
                Arg.Is<GenerateQRCodeCommand>(
                    c => c.CustId == command.CustId
                    && c.SubAccId == command.SubAccId
                    && c.CustomerLevel == command.CustomerLevel
                    && c.Language == command.Language));
        }

        [Test]
        public async Task VerifyOTP_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new VerifyOTPCommand(userSession);
            var expectedResult = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var actualResult = await controller.VerifyOTP(command) as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedResult, actualResult.Data).AreEqual);
        }

        [Test]
        public async Task ResendOTP_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new ResendOTPCommand(userSession);
            var expectedResult = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var actualResult = await controller.ResendOTP(command) as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedResult, actualResult.Data).AreEqual);
        }

        [Test]
        public async Task ResendOTP_IsNotInternal_CallsMediatorResendOTPCommand()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new ResendOTPCommand(userSession);

            // Act
            await controller.ResendOTP(command);

            // Assert
            await mediator.Received().SendAsync(
                Arg.Is<ResendOTPCommand>(
                    c => c.CustId == command.CustId
                    && c.SubAccId == command.SubAccId));
        }

        [TestCase("en-us")]
        [TestCase("ja-jp")]
        [TestCase("ko-kr")]
        [TestCase("th-th")]
        public void Language_Always_CallsMediatorSendTelegramBotLanguageQuery(string langCode)
        {
            // Act
            var query = new TelegramBotLanguageQuery(langCode, AppSettings.DisabledVietnamese);
            controller.Language(langCode);

            // Assert
            mediator.Received().Send(
                Arg.Is<TelegramBotLanguageQuery>(
                    param => param.LanguageCode == query.LanguageCode
                    && param.DisabledVietnamese == query.DisabledVietnamese));
        }

        [Test]
        public async Task UpdateFunction_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new UpdateFunctionCommand(userSession);
            var expectedResult = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var actualResult = await controller.UpdateFunction(command) as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedResult, actualResult.Data).AreEqual);
        }

        [Test]
        public async Task UpdateFunction_IsNotInternal_CallsMediatorUpdateFunctionCommand()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new UpdateFunctionCommand(userSession);

            // Act
            await controller.UpdateFunction(command);

            // Assert
            await mediator.Received().SendAsync(
                Arg.Is<UpdateFunctionCommand>(
                    c => c.CustId == command.CustId
                    && c.SubAccId == command.SubAccId
                    && c.FunctionId == command.FunctionId
                    && c.FunctionName == command.FunctionName
                    && c.Type == command.Type
                    && c.Enabled == command.Enabled
                    && c.Language == command.Language
                    && c.IP == command.IP));
        }

        [Test]
        public void Captcha_Always_ReturnsPartialView()
        {
            // Act
            var result = controller.Captcha();

            // Assert
            Assert.IsTrue(result is PartialViewResult);
        }

        [Test]
        public async Task UpdateLanguage_IsInternal_ReturnsUnauthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new UpdateLanguageCommand(userSession);
            var expectedResult = UnauthorizedResult.CreateUnauthorizedResult();

            // Act
            var actualResult = await controller.UpdateLanguage(command) as JsonResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedResult, actualResult.Data).AreEqual);
        }

        [Test]
        public async Task UpdateLanguage_IsNotInternal_CallsMediatorUpdateLanguageCommand()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            var command = new UpdateLanguageCommand(userSession) 
            { 
                Language = "en-US"
            };

            // Act
            await controller.UpdateLanguage(command);

            // Assert
            await mediator.Received().SendAsync(
                Arg.Is<UpdateLanguageCommand>(
                    c => c.CustId == command.CustId
                    && c.SubAccId == command.SubAccId
                    && c.UserLanguage == command.UserLanguage
                    && c.Language == command.Language
                    && c.IP == command.IP));
        }
    }
}
