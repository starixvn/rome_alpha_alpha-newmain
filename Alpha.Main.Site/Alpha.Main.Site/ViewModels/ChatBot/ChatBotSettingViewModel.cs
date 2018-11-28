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
            ChatBotViewModel = chatbotViewModel;
            lestalkAccount = chatbotViewModel.User.MessengerAccounts?.FirstOrDefault(m => m.Type == MessengerType.Letstalk && m.Enabled);
            Language = chatbotViewModel.User.Language;

            var selfdestructionPeriod = chatbotViewModel.User.SelfdestructionPeriod.HasValue
                ? chatbotViewModel.User.SelfdestructionPeriod.Value
                : Constants.SelfdestructionPeriod.ThreeHours;

            SelfdestructionPeriodOptions = Constants.SelfdestructionPeriod.Options
                .Select(o => new SelectListItem
                {
                    Text = ChatBot.ResourceManager.GetString(o.Value),
                    Value = o.Key.ToString(),
                    Selected = o.Key == selfdestructionPeriod
                });
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string Language { get; }

        public string LetstalkId => lestalkAccount.Username;

        public string Title => ChatBot.ChatBotTitle;

        public IEnumerable<SelectListItem> SelfdestructionPeriodOptions { get; }
    }
}