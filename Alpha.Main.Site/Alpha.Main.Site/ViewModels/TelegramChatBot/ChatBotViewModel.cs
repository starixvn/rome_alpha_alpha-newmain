namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using TelegramBot.Client.Models;

    public class ChatBotViewModel
    {
        public ChatBotViewModel(CustBotMessenger custBotMessenger, bool isEnableAutoTransfer = false)
        {
            CustBotMessenger = custBotMessenger;
            IsEnableAutoTransfer = isEnableAutoTransfer;
        }

        public CustBotMessenger CustBotMessenger { get; }

        public bool IsEnableAutoTransfer { get; }

        public bool IsLinked => CustBotMessenger != null && CustBotMessenger.IsVerifyOTP && CustBotMessenger.Enabled;
    }
}