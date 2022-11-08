using Alpha.Main.Configurations;
using Alpha.Main.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TelegramBot.Client.Models;
using TelegramBot.Client.Services;

namespace Alpha.Main.Site.Helpers
{
    public interface ITelegramBotHelper
    {
        Task<Tuple<BotEntity, CustBotMessenger>> GetTelegramBotAndCustMessengerAsync(int custId, int subAccId, string language);

        Task<BotEntity> GetTelegramBotAsync();
    }

    public class TelegramBotHelper : ITelegramBotHelper
    {
        private readonly ISettingService telegramSettingService;
        private readonly IBotService botService;

        public TelegramBotHelper(
            ISettingService settingService,
            IBotService botService)
        {
            this.telegramSettingService = settingService;
            this.botService = botService;
        }
        public async Task<Tuple<BotEntity, CustBotMessenger>> GetTelegramBotAndCustMessengerAsync(int custId, int subAccId, string language)
        {
            var botTask = GetTelegramBotAsync();
            var custBotMessengerTask = telegramSettingService.GetCustBotMessengerAsync(
                custId,
                subAccId,
                Language.MapNewChineseLanguageCode(language));

            await Task.WhenAll(botTask, custBotMessengerTask);

            return new Tuple<BotEntity, CustBotMessenger>(await botTask, await custBotMessengerTask);
        }

        public async Task<BotEntity> GetTelegramBotAsync()
        {
            return await botService.GetBySystem(AppSettings.SiteGroup);
        }
    }
}