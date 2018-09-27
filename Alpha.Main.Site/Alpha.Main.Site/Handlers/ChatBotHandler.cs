namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private const string ChatbotUserIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.UserId";
        private const string LetstalkIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.LetstalkId";
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
        {
            var user = await settingService.GetUser(message.CustId, message.SubAccountId, message.Language);

            if (user != null)
            {
                sessionManager.Add(ChatbotUserIdSessionKey, user.Id);

                var letstalkAccount = user.MessengerAccounts.FirstOrDefault(
                    account => account.Type == MessengerType.Letstalk
                            && (account.Enabled || account.IsVerifying));

                if (letstalkAccount != null)
                {
                    sessionManager.Add(LetstalkIdSessionKey, letstalkAccount.Username);
                }
            }

            return new ChatBotViewModel(user);
        }

        public async Task<AddingUserResult> Handle(AddAccountCommand message)
        {
            var result = await settingService.AddUser(
                 message.CustId,
                 message.CustLevel,
                 message.SubAccountId,
                 message.Language,
                 message.LetstalkId);

            if (result.IsOk)
            {
                sessionManager.Add(ChatbotUserIdSessionKey, result.UserId);
                sessionManager.Add(LetstalkIdSessionKey, message.LetstalkId);
            }

            return result;
        }

        public async Task<Result> Handle(UnsubscribeCommand message)
            => await settingService.Unsubscribe(
                sessionManager.Get<int>(ChatbotUserIdSessionKey),
                MessengerType.Letstalk,
                sessionManager.Get<string>(LetstalkIdSessionKey));

        public async Task<VerificationCodeResult> Handle(VerifyVerificationCodeCommand message)
            => await settingService.VerifyVerificationCode(
                sessionManager.Get<int>(ChatbotUserIdSessionKey),
                message.VerificationCode,
                sessionManager.Get<string>(LetstalkIdSessionKey));

        public async Task<Result> Handle(ResendVerificationCodeCommand message)
        {
            var result = new Result();
            var lastResendTime = sessionManager.Get<DateTime>(LastResendTimeKey);

            if (lastResendTime.AddSeconds(message.IntervalThresholdToSendRequestInSeconds) < DateTime.Now)
            {
                result = await settingService.GenerateVerificationCode(
                    sessionManager.Get<int>(ChatbotUserIdSessionKey),
                    sessionManager.Get<string>(LetstalkIdSessionKey));

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

            return await settingService.UpdateFunctions(
                sessionManager.Get<int>(ChatbotUserIdSessionKey),
                functions);
        }

        public async Task<Result> Handle(UpdateLanguageCommand message)
            => await settingService.UpdateLanguage(
                sessionManager.Get<int>(ChatbotUserIdSessionKey),
                message.Language);
    }
}