namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class ChatBotViewModel
    {
        public ChatBotViewModel(string user)
        {
            User = user;
        }

        public string User { get; }
    }
}