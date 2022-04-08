namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, string language, string custLevel, bool isViewReports, bool isViewMemberInfo)
        {
            ChatBotViewModel = chatBotViewModel;

            TelegramId = chatBotViewModel.CustBotMessenger.MessengerName;
            ChatBotName = chatBotViewModel.CustBotMessenger.BotName;
            Language = language;
            CustLevel = custLevel;

            IsViewReports = isViewReports;
            IsViewMemberInfo = isViewMemberInfo;
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