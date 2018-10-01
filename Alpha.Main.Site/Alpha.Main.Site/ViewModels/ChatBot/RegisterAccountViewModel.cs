namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Linq;
    using AppResources;
    using Chatbot.Client.Models;

    public class RegisterAccountViewModel
    {
        private readonly Messenger letstalkAccount;

        public RegisterAccountViewModel(User user)
        {
            letstalkAccount = user?.MessengerAccounts?.FirstOrDefault(m => m.Type == MessengerType.Letstalk && m.IsVerifying);
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
    }
}