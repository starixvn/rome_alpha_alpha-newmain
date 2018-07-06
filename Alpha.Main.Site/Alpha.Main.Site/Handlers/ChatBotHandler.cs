namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Chatbot.Client.Models;
    using Chatbot.Client.Services;
    using MediatR;
    using Requests.ChatBot;
    using ViewModels.ChatBot;

    public class ChatBotHandler :
        IAsyncRequestHandler<SetUpChatBotQuery, ChatBotViewModel>,
        IAsyncRequestHandler<AddAccountCommand, AddingUserResult>,
        IAsyncRequestHandler<UnsubscribeCommand, Result>,
        IAsyncRequestHandler<VerifyVerificationCodeCommand, VerificationCodeResult>,
        IAsyncRequestHandler<ResendVerificationCodeCommand, Result>
    {
        private readonly SettingService settingService;

        public ChatBotHandler(SettingService settingService)
        {
            this.settingService = settingService;
        }

        public async Task<ChatBotViewModel> Handle(SetUpChatBotQuery message)
            => new ChatBotViewModel(await settingService.GetUser(message.CustId, message.SubAccountId, message.Language));

        public async Task<AddingUserResult> Handle(AddAccountCommand message)
            => await settingService.AddUser(
                message.CustId,
                message.SubAccountId,
                message.Language,
                message.MessengerAccount);

        public async Task<Result> Handle(UnsubscribeCommand message)
            => await settingService.Unsubscribe(message.UserId, message.MessengerType);

        public async Task<VerificationCodeResult> Handle(VerifyVerificationCodeCommand message)
            => await settingService.VerifyVerificationCode(
                message.UserId,
                message.VerificationCode,
                message.MessengerAccount,
                message.MessengerType);

        public Task<Result> Handle(ResendVerificationCodeCommand message)
        {
            throw new NotImplementedException();
        }
    }
}