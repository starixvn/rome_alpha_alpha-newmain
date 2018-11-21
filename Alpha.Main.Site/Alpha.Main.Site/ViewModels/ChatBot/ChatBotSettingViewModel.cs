namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Chatbot.Client.Models;

    public class ChatBotSettingViewModel
    {
        private readonly Messenger lestalkAccount;

        public ChatBotSettingViewModel(ChatBotViewModel chatbotViewModel)
        {
            lestalkAccount = chatbotViewModel.User.MessengerAccounts?.FirstOrDefault(m => m.Type == MessengerType.Letstalk && m.Enabled);
            ChatBotViewModel = chatbotViewModel;
        }

        public string Language => ChatBotViewModel.User.Language;

        public string LetstalkId => lestalkAccount.Username;

        public string Title => ChatBot.ChatBotTitle;

        public ChatBotViewModel ChatBotViewModel { get; }

        public int SelfdestructionPeriod
            => ChatBotViewModel.User.SelfdestructionPeriod.HasValue
            ? ChatBotViewModel.User.SelfdestructionPeriod.Value
            : Constants.SelfdestructionPeriod.ThreeHours;

        public IDictionary<int, string> SelfdestructionPeriodOptions
            => Constants.SelfdestructionPeriod.Options;
    }
}