namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Web.Helpers;
    using System.Collections.Generic;
    using System.Linq;
    using TelegramBot.Client.Models;

    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;

            UserId = chatBotViewModel.CustBotMessenger.UserId;
            ChatId = chatBotViewModel.CustBotMessenger.ChatId;
            ChatBotName = chatBotViewModel.CustBotMessenger.BotName;
            ReportFunctions = chatBotViewModel.CustBotMessenger.Functions.Where(function => function.Type == 1);
            NotificationFunctions = chatBotViewModel.CustBotMessenger.Functions.Where(function => function.Type == 2);
            Language = chatBotViewModel.CustBotMessenger.Language;
            CustLevel = query.CustLevel.DisplayName;
            IsEnableAutoTransfer = chatBotViewModel.IsEnableAutoTransfer;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public int UserId { get; }

        public long ChatId { get; }

        public string ChatBotName { get; }

        public IEnumerable<Function> ReportFunctions { get; }

        public IEnumerable<Function> NotificationFunctions { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsEnableAutoTransfer { get; }

        public string HowToUseLink
            => AppSettings.IsSiteLabel789y ?
                MicroSites.MessageUrl("~/StaticMessage/Guide?id=167819") :
                MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=167819");
    }
}