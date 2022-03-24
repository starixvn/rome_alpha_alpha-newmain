namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, string language, string custLevel)
        {
            ChatBotViewModel = chatBotViewModel;
            Language = language;
            CustLevel = custLevel;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string TelegramId => "Test";

        public string Language { get; }

        public string CustLevel { get; }
    }
}