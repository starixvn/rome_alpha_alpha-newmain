namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using System.Collections.Generic;
    using TelegramBot.Client.Models;

    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;

            UserId = chatBotViewModel.CustBotMessenger.UserId;
            ChatId = chatBotViewModel.CustBotMessenger.ChatId;
            ChatBotName = chatBotViewModel.CustBotMessenger.BotName;
            Functions = chatBotViewModel.CustBotMessenger.Functions;
            Language = chatBotViewModel.CustBotMessenger.Language;

            CustLevel = query.CustLevel.DisplayName;
            IsViewReports = query.IsViewReports;
            IsViewMemberInfo = query.IsViewMemberInfo;
            IsShowLetstalkChatBot = query.IsShowLetstalkChatBot;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public int UserId { get; }

        public long ChatId { get; }

        public string ChatBotName { get; }

        public IEnumerable<Function> Functions { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsViewReports { get; }

        public bool IsViewMemberInfo { get; }

        public bool IsShowLetstalkChatBot { get; }
    }
}