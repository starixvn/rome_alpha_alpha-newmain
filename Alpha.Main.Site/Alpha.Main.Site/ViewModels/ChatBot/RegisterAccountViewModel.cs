namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Linq;
    using AppResources;
    using Chatbot.Client.Models;

    public class RegisterAccountViewModel
    {
        private readonly Messenger letstalkAccount;

        public RegisterAccountViewModel(ChatBotViewModel chatbotViewModel, string language, string custLevel)
        {
            letstalkAccount = chatbotViewModel.User?.MessengerAccounts?.FirstOrDefault(m => m.Type == MessengerType.Letstalk && m.IsVerifying);
            ChatBotViewModel = chatbotViewModel;
            Language = language;
            CustLevel = custLevel;
        }

        public string LetstalkId
            => !string.IsNullOrWhiteSpace(letstalkAccount?.Username)
                ? letstalkAccount.Username
                : string.Empty;

        public bool IsVerifying => letstalkAccount != null;

        public VerifyAccountViewModel VerifyAccountViewModel
            => IsVerifying
                ? new VerifyAccountViewModel(letstalkAccount.InputWrongVerificationCodeTimes >= 3)
                : null;

        public string Title => ChatBot.ChatBotTitle;

        public ChatBotViewModel ChatBotViewModel { get; }

        public string Language { get; }

        public string CustLevel { get; }
    }
}