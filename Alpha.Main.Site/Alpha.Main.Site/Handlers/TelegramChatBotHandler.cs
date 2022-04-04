namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.Helpers;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using MediatR;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TelegramBot.Client.Services;
    using TelegramBot.Client.Models;
    using Fanex.Caching;

    public class TelegramChatBotHandler :
        IAsyncRequestHandler<SetUpChatBotQuery, ChatBotViewModel>,
        IAsyncRequestHandler<GenerateQRCodeCommand, VerifyAccountViewModel>,
        IAsyncRequestHandler<VerifyOTPCommand, Result>,
        IAsyncRequestHandler<ResendOTPCommand, Result>,
        IAsyncRequestHandler<UnsubscribeCommand, Result>
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
            var custBotMessenger = await settingService.GetCustBotMessengerAsync(message.CustId, message.SubAccId, AppSettings.LocalTest);

            if (custBotMessenger == null)
            {
                return new ChatBotViewModel(null);
            }

            return new ChatBotViewModel(custBotMessenger); 
        }

        public async Task<VerifyAccountViewModel> Handle(GenerateQRCodeCommand message)
        {
            var url = await settingService.GenerateStartUriAsync(message.CustId, message.SubAccId, AppSettings.SiteGroup, AppSettings.LocalTest);

            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets/styles/telegramchatbot/images/telegram-sm.png");
            var qrCodeDataUrl = QRCodeAdapter.Generate(url, logoPath);

            return new VerifyAccountViewModel(url, qrCodeDataUrl);
        }

        public async Task<Result> Handle(VerifyOTPCommand message)
        {
            var bot = await GetTelegramBotAsync();
            return await settingService.LinkAsync(bot.BotId, message.CustId, message.SubAccId, message.OTP);
        }

        public async Task<Result> Handle(ResendOTPCommand message)
        {
            var bot = await GetTelegramBotAsync();
            return await settingService.ResendOtpAsync(bot.BotId, message.CustId, message.SubAccId, AppSettings.LocalTest);
        }

        public async Task<Result> Handle(UnsubscribeCommand message)
        {
            var bot = await GetTelegramBotAsync();
            return await settingService.UnlinkAsync(bot.BotId, message.CustId, message.SubAccId);
        }

        private async Task<BotEntity> GetTelegramBotAsync()
        {
            var telegramBot = cacheService.Get<BotEntity>(TelegramBotKey);

            if (telegramBot == null)
            {
                var allBots = await botService.GetAllAsync();
                telegramBot = allBots.FirstOrDefault(b => b.IsTest == AppSettings.LocalTest && b.System == AppSettings.SiteGroup);
                cacheService.Set(TelegramBotKey, telegramBot);
            }

            return telegramBot;
        }
    }
}