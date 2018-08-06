namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Linq;
    using AppResources;
    using Chatbot.Client.Models;
    using Constants;

    public class RegisterAccountViewModel
    {
        private readonly User user;
        private readonly Messenger letstalkMessenger;

        public RegisterAccountViewModel(User user)
        {
            this.user = user;
            letstalkMessenger = user?.MessengerAccounts?.FirstOrDefault(m => m.Type == MessengerTypeConstants.Letstalk && m.IsVerifying);
        }

        public string Username
            => !string.IsNullOrWhiteSpace(letstalkMessenger?.Username)
                ? letstalkMessenger.Username
                : string.Empty;

        public bool IsVerifying => letstalkMessenger != null;

        public VerifyAccountViewModel VerifyAccountViewModel
            => IsVerifying
                ? new VerifyAccountViewModel(user.Id, Username, letstalkMessenger.InputWrongVerificationCodeTimes >= 3)
                : null;

        public string Title => ChatBot.ChatBotTitle;
    }
}