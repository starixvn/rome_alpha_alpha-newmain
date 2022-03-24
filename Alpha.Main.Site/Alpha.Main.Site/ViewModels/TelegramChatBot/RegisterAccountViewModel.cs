namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class RegisterAccountViewModel 
    {
        private readonly string telegramAccount;

        public RegisterAccountViewModel(ChatBotViewModel chatBotViewModel, string language, string custLevel)
        {
            ChatBotViewModel = chatBotViewModel;
            Language = language;
            CustLevel = custLevel;
        }

        public bool IsVerifying => telegramAccount != null;

        public VerifyAccountViewModel VerifyAccountViewModel
            => IsVerifying
                ? new VerifyAccountViewModel("https://www.telegram.org/", "/Assets/styles/chatbot/images/qrcode.png")
                : null;

        public ChatBotViewModel ChatBotViewModel { get; }

        public string Language { get; }

        public string CustLevel { get; }
    }
}
