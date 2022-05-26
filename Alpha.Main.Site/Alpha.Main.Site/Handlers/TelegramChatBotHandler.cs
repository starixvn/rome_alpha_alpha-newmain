namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Helpers;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using MediatR;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using TelegramBot.Client.Models;
    using Fanex.Caching;
    using TelegramBot.Client.Services;
    using Alpha.Main.Configurations;

    public class TelegramChatBotHandler :
        IAsyncRequestHandler<SetUpChatBotQuery, ChatBotViewModel>,
        IAsyncRequestHandler<GenerateQRCodeCommand, VerifyAccountViewModel>,
        IAsyncRequestHandler<VerifyOTPCommand, Result>,
        IAsyncRequestHandler<ResendOTPCommand, Result>,
        IAsyncRequestHandler<UnsubscribeCommand, Result>,
        IAsyncRequestHandler<UpdateFunctionCommand, Result>,
        IAsyncRequestHandler<UpdateLanguageCommand, Result>
    {
        private const string TelegramBotKey = "Alpha.Main.Site.Handlers.TelegramChatBotHandler.TelegramBot";
        private readonly ICacheService cacheService;
        private readonly ISettingService settingService;
        private readonly IBotService botService;

        public TelegramChatBotHandler(ICacheService cacheService, ISettingService settingService, IBotService botService)
        {
            this.cacheService = cacheService;
            this.settingService = settingService;
            this.botService = botService;
        }

        public async Task<ChatBotViewModel> Handle(SetUpChatBotQuery message)
        {
            var custBotMessenger = await settingService.GetCustBotMessengerAsync(message.CustId, message.SubAccId);
            return new ChatBotViewModel(custBotMessenger); 
        }

        public async Task<VerifyAccountViewModel> Handle(GenerateQRCodeCommand message)
        {
            var url = await settingService.GenerateStartUriAsync(message.CustId, message.SubAccId, AppSettings.SiteGroup, message.Language);

            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets/styles/telegramchatbot/images/telegram-logo.png");
            var qrCodeDataUrl = QRCodeAdapter.Generate(url, logoPath);

            return new VerifyAccountViewModel(url, qrCodeDataUrl);
        }

        public async Task<Result> Handle(VerifyOTPCommand message)
        {
            var bot = await GetTelegramBotAsync();
            return await settingService.LinkAsync(bot.BotId, message.CustId, message.SubAccId, message.OTP, message.Username, message.IP);
        }

        public async Task<Result> Handle(ResendOTPCommand message)
        {
            return await settingService.ResendOtpAsync(message.CustId, message.SubAccId);
        }

        public async Task<Result> Handle(UnsubscribeCommand message)
        {
            var bot = await GetTelegramBotAsync();
            return await settingService.UnlinkAsync(bot.BotId, message.CustId, message.SubAccId, message.IP);
        }

        public async Task<Result> Handle(UpdateFunctionCommand message)
        {
            var function = new Function
            {
                FunctionId = message.FunctionId,
                FunctionName = message.FunctionName,
                Enabled = message.Enabled,
                Type = message.Type
            };
            return await settingService.UpdateUserFunction(message.UserId, function, message.CustId, message.IP);
        }

        public async Task<Result> Handle(UpdateLanguageCommand message)
            => await settingService.UpdateLanguage(message.UserId, message.Language, message.IP);

        private async Task<BotEntity> GetTelegramBotAsync()
        {
            var telegramBot = cacheService.Get<BotEntity>(TelegramBotKey);

            if (telegramBot == null)
            { 
                telegramBot = await botService.GetBySystem(AppSettings.SiteGroup);
                cacheService.Set(TelegramBotKey, telegramBot);
            }

            return telegramBot;
        }
    }
}