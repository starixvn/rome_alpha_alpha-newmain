namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using System.Linq;
    using TelegramBot.Client.Models;

    public class RegisterAccountViewModel
    {
        public RegisterAccountViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;
            Language = query.Language;
            CustLevel = query.CustLevel.DisplayName;
            IsShowLetstalkChatBot = query.IsShowLetstalkChatBot;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsShowLetstalkChatBot { get; }

        public bool IsOtpPage { get; set; }

        public Function OtpFunction => ChatBotViewModel.CustBotMessenger.Functions.FirstOrDefault(func => func.FunctionId == 15);

        public bool IsEnableOTP => ChatBotViewModel.CustBotMessenger.Functions.Any(func => func.FunctionId == 15 && func.Enabled == true);
    }
}
