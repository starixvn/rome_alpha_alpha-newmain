namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Linq;
    using Chatbot.Client.Models;
    using Constants;

    public class ChatBotViewModel
    {
        public ChatBotViewModel(User user)
        {
            User = user;
        }

        public User User { get; }

        public bool IsAlreadySetup
            => User != null && User.MessengerAccounts.Any(m => m.Type == MessengerTypeConstants.Letstalk && m.Enabled);
    }
}