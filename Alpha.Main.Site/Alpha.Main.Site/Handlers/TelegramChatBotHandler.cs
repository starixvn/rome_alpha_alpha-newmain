namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Alpha.Main.Configurations;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Helpers;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using MediatR;
    using TelegramBot.Client.Models;
    using TelegramBot.Client.Services;

    public class TelegramChatBotHandler :
        IAsyncRequestHandler<SetUpChatBotQuery, ChatBotViewModel>,
        IAsyncRequestHandler<GenerateQRCodeCommand, VerifyAccountViewModel>,
        IAsyncRequestHandler<VerifyOTPCommand, Result>,
        IAsyncRequestHandler<ResendOTPCommand, Result>,
        IAsyncRequestHandler<UnsubscribeCommand, Result>,
        IAsyncRequestHandler<UpdateFunctionCommand, Result>,
        IAsyncRequestHandler<UpdateLanguageCommand, Result>
    {
        private readonly ISettingService settingService;
        private readonly IBotService botService;

        public TelegramChatBotHandler(ISettingService settingService, IBotService botService)
        {

            this.settingService = settingService;
            this.botService = botService;
        }

        public async Task<ChatBotViewModel> Handle(SetUpChatBotQuery message)
        {
            var custBotMessenger = await settingService.GetCustBotMessengerAsync(
                message.CustId,
                message.SubAccId,
                Language.MapNewChineseLanguageCode(message.Language));

            return new ChatBotViewModel(custBotMessenger); 
        }

        public async Task<VerifyAccountViewModel> Handle(GenerateQRCodeCommand message)
        {
            var url = await settingService.GenerateStartUriAsync(
                message.CustId,
                message.SubAccId,
                AppSettings.SiteGroup,
                Language.MapNewChineseLanguageCode(message.Language));

            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets/styles/telegramchatbot/images/telegram-logo.png");
            var qrCodeDataUrl = QRCodeAdapter.Generate(url, logoPath);

            return new VerifyAccountViewModel(url, qrCodeDataUrl);
        }

        public async Task<Result> Handle(VerifyOTPCommand message)
        {
            var bot = await GetTelegramBotAsync();
            var username = message.IsSubAcc ? message.SubUserName : message.Username;
            return await settingService.LinkAsync(bot.BotId, message.CustId, message.SubAccId, message.OTP, username, message.IP);
        }

        public async Task<Result> Handle(ResendOTPCommand message)
        {
            return await settingService.ResendOtpAsync(message.CustId, message.SubAccId);
        }

        public async Task<Result> Handle(UnsubscribeCommand message)
            => await settingService.UnlinkAsync(message.CustId, message.SubAccId, message.IP);

        public async Task<Result> Handle(UpdateFunctionCommand message)
        {
            var botTask = GetTelegramBotAsync();
            var custBotMessengerTask = settingService.GetCustBotMessengerAsync(
                message.CustId,
                message.SubAccId,
                Language.MapNewChineseLanguageCode(message.Language));
            await Task.WhenAll(botTask, custBotMessengerTask);

            var bot = await botTask;
            var custBotMessenger = await custBotMessengerTask;

            var function = new Function
            {
                FunctionId = message.FunctionId,
                FunctionName = message.FunctionName,
                Enabled = message.Enabled,
                Type = message.Type
            };
            return await settingService.UpdateUserFunction(custBotMessenger.UserId, bot.BotId, function, message.CustId, message.IP);
        }

        public async Task<Result> Handle(UpdateLanguageCommand message)
        {
            var botTask = GetTelegramBotAsync();
            var custBotMessengerTask = settingService.GetCustBotMessengerAsync(
                message.CustId,
                message.SubAccId,
                Language.MapNewChineseLanguageCode(message.UserLanguage));
            await Task.WhenAll(botTask, custBotMessengerTask);

            var bot = await botTask;
            var custBotMessenger = await custBotMessengerTask;

            return await settingService.UpdateLanguage(
                custBotMessenger.UserId,
                bot.BotId,
                Language.MapNewChineseLanguageCode(message.Language),
                message.CustId,
                message.IP);
        }

        private async Task<BotEntity> GetTelegramBotAsync()
        {
            return await botService.GetBySystem(AppSettings.SiteGroup);
        }
    }
}