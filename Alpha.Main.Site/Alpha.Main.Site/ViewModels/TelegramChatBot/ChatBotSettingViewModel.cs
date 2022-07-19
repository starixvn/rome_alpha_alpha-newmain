namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Web.Helpers;
    using Sunplus.Agent.Model.Entities;
    using System;
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
            IsShowLetstalkChatBot = query.IsShowLetstalkChatBot;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public int UserId { get; }

        public long ChatId { get; }

        public string ChatBotName { get; }

        public IEnumerable<Function> ReportFunctions { get; }

        public IEnumerable<Function> NotificationFunctions { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsShowLetstalkChatBot { get; }

        public string HowToUseLink 
            => UserSession.Current.Site.Equals("789y", StringComparison.OrdinalIgnoreCase) ?
                MicroSites.MessageUrl("~/StaticMessage/Guide?id=167819") :
                MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=167819");
    }
}