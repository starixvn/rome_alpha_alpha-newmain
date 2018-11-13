namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using System.Linq;
    using Chatbot.Client.Models;
    using Constants;

    public class ChatBotViewModel
    {
        public ChatBotViewModel(User user, IEnumerable<Function> functions)
        {
            User = user;
            ReportFunctions = functions.Where(f => f.Type == ChatBotFunctionType.Report);
            NotificationFunctions = functions.Where(f => f.Type == ChatBotFunctionType.Notification);
        }

        public User User { get; }

        public bool IsAlreadySetup
            => User != null && User.MessengerAccounts.Any(m => m.Type == MessengerType.Letstalk && m.Enabled);

        public IEnumerable<Function> ReportFunctions { get; }

        public IEnumerable<Function> NotificationFunctions { get; }
    }
}