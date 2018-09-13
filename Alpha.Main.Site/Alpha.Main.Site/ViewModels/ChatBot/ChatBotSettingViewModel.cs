namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Chatbot.Client.Models;
    using Constants;

    public class ChatBotSettingViewModel
    {
        private readonly User user;
        private readonly Messenger lestalkAccount;

        public ChatBotSettingViewModel(User user)
        {
            this.user = user;
            lestalkAccount = user.MessengerAccounts?.FirstOrDefault(m => m.Type == MessengerTypeConstants.Letstalk && m.Enabled);
        }

        public string Language => user.Language;

        public string Username => lestalkAccount.Username;

        public IEnumerable<Function> ReportFunctions => user.Functions?.Where(f => f.Type == ChatBotFunctionTypeConstants.Report);

        public IEnumerable<Function> NotificationFunctions => user.Functions?.Where(f => f.Type == ChatBotFunctionTypeConstants.Notification);

        public string Title => ChatBot.ChatBotTitle;
    }
}