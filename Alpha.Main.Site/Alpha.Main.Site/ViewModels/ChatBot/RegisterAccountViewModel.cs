namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Linq;
    using Chatbot.Client.Models;

    public class RegisterAccountViewModel
    {
        private readonly Messenger letstalkMessenger;

        public RegisterAccountViewModel(User user)
        {
            User = user;
            letstalkMessenger = user?.MessengerAccounts?.FirstOrDefault(m => m.Type == 1);
        }

        public User User { get; }

        public string MessengerAccount
            => !string.IsNullOrWhiteSpace(letstalkMessenger?.Account)
                ? letstalkMessenger.Account
                : string.Empty;

        public bool IsVerifying => letstalkMessenger != null && letstalkMessenger.IsVerifying;

        public VerifyAccountViewModel VerifyAccountViewModel
            => IsVerifying
                ? new VerifyAccountViewModel(User.Id, MessengerAccount)
                : null;
    }
}