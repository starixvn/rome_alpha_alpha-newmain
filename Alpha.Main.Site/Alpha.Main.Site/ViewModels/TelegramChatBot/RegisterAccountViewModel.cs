namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Site.Requests.TelegramChatBot;

    public class RegisterAccountViewModel 
    {
        public RegisterAccountViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;
            Language = query.Language;
            CustLevel = query.CustLevel.DisplayName;
            IsShowLetstalkChatBot = query.IsShowLetstalkChatBot;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsShowLetstalkChatBot { get; }
    }
}
