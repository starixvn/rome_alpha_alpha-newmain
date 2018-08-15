namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Chatbot.Client.Models;
    using Chatbot.Client.Services;
    using MediatR;
    using Requests.ChatBot;
    using ViewModels.ChatBot;
    using Web.Helpers;

    public class ChatBotHandler :
        IAsyncRequestHandler<SetUpChatBotQuery, ChatBotViewModel>,
        IAsyncRequestHandler<AddAccountCommand, AddingUserResult>,
        IAsyncRequestHandler<UnsubscribeCommand, Result>,
        IAsyncRequestHandler<VerifyVerificationCodeCommand, VerificationCodeResult>,
        IAsyncRequestHandler<ResendVerificationCodeCommand, Result>,
        IAsyncRequestHandler<UpdateFunctionCommand, Result>,
        IAsyncRequestHandler<UpdateLanguageCommand, Result>
    {
        private const string LastResendTimeKey = "Alpha.Main.Site.Handlers.ChatBotHandler.LastResendTimeKey";
        private readonly SettingService settingService;
        private readonly SessionManager sessionManager;

        public ChatBotHandler(
            SettingService settingService,
            SessionManager sessionManager)
        {
            this.settingService = settingService;
            this.sessionManager = sessionManager;
        }

        public async Task<ChatBotViewModel> Handle(SetUpChatBotQuery message)
            => new ChatBotViewModel(await settingService.GetUser(message.CustId, message.SubAccountId, message.Language));

        public async Task<AddingUserResult> Handle(AddAccountCommand message)
            => await settingService.AddUser(
                message.CustId,
                message.SubAccountId,
                message.CustName,
                message.Language,
                message.MessengerAccount);

        public async Task<Result> Handle(UnsubscribeCommand message)
            => await settingService.Unsubscribe(message.UserId, message.MessengerType, message.MessengerUserName);

        public async Task<VerificationCodeResult> Handle(VerifyVerificationCodeCommand message)
            => await settingService.VerifyVerificationCode(
                message.UserId,
                message.VerificationCode,
                message.MessengerAccount,
                message.MessengerType);

        public async Task<Result> Handle(ResendVerificationCodeCommand message)
        {
            var result = new Result();
            var lastResendTime = sessionManager.Get<DateTime>(LastResendTimeKey);

            if (lastResendTime.AddSeconds(message.IntervalThresholdToSendRequestInSeconds) < DateTime.Now)
            {
                result = await settingService.GenerateVerificationCode(message.UserId, message.CustName, message.MessengerUsername);
                sessionManager.Add(LastResendTimeKey, DateTime.Now);
            }

            return result;
        }

        public async Task<Result> Handle(UpdateFunctionCommand message)
        {
            var functions = new List<Function>
            {
                new Function {
                    Id = message.FunctionId,
                    Command=message.Command,
                    Enabled=message.IsEnable
                }
            };

            return await settingService.UpdateFunctions(message.UserId, functions);
        }

        public async Task<Result> Handle(UpdateLanguageCommand message)
            => await settingService.UpdateLanguage(message.UserId, message.Language);
    }
}