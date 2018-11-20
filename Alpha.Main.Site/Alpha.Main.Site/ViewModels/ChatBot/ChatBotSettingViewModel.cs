namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
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

        public int MessageSelfDestructTime
            => ChatBotViewModel.User.SelfdestructionPeriod.HasValue
            ? ChatBotViewModel.User.SelfdestructionPeriod.Value
            : Constants.SelfdestructionPeriod.ThreeHours;

        public IEnumerable<SelectListItem> SelfdestructionPeriodOptions
            => Constants.SelfdestructionPeriod.Options.Select(o => new SelectListItem
            {
                Text = o.Value,
                Value = o.Key.ToString(),
                Selected = o.Key == MessageSelfDestructTime
            });
    }
}