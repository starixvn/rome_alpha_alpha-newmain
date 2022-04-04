namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, string language, string custLevel)
        {
            ChatBotViewModel = chatBotViewModel;
            Language = language;
            CustLevel = custLevel;
            TelegramId = chatBotViewModel.CustBotMessenger.MessengerName;
            ChatBotName = chatBotViewModel.CustBotMessenger.BotName;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string TelegramId { get; }

        public string ChatBotName { get; }

        public string Language { get; }

        public string CustLevel { get; }
    }
}