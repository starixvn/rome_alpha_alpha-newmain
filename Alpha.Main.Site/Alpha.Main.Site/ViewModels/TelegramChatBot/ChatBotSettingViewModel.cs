namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Site.Requests.TelegramChatBot;

    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;

            TelegramId = chatBotViewModel.CustBotMessenger.MessengerName;
            ChatBotName = chatBotViewModel.CustBotMessenger.BotName;
            Language = query.Language;
            CustLevel = query.CustLevel.DisplayName;

            IsViewReports = query.IsViewReports;
            IsViewMemberInfo = query.IsViewMemberInfo;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string TelegramId { get; }

        public string ChatBotName { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsViewReports { get; }

        public bool IsViewMemberInfo { get; }
    }
}