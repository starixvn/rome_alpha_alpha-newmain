namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using TelegramBot.Client.Models;

    public class ChatBotViewModel
    {
        public ChatBotViewModel(CustBotMessenger custBotMessenger)
        {
            CustBotMessenger = custBotMessenger;
        }

        public CustBotMessenger CustBotMessenger { get; }

        public bool IsLinked => CustBotMessenger != null && CustBotMessenger.IsVerifyOTP && CustBotMessenger.Status;
    }
}