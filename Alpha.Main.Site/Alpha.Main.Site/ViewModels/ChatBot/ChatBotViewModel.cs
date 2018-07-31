namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Chatbot.Client.Models;

    public class ChatBotViewModel
    {
        public ChatBotViewModel(User user)
        {
            User = user;
        }

        public User User { get; }

        public bool IsAlreadySetup
            => User != null
                && User.MessengerAccounts != null
                && User.MessengerAccounts.Any(m => !m.IsVerifying && m.Enabled);

        public RegisterAccountViewModel RegisterAccountViewModel => new RegisterAccountViewModel(User);

        public string Title => ChatBot.ChatBotTitle;

        public IEnumerable<Function> ReportFunctions => User?.Functions?.Where(f => f.Type == 1);

        public IEnumerable<Function> NotificationFunctions => User?.Functions?.Where(f => f.Type == 2);
    }
}