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
        private const string LetstalkAccountEnabledStatusKey = "Alpha.Main.Site.Handlers.ChatBotHandler.Status.Enabled";
        private const string LetstalkAccountIsVerifyingStatusKey = "Alpha.Main.Site.Handlers.ChatBotHandler.Status.IsVerifying";
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

            if (user == null)
            {
                return new ChatBotViewModel(user);
            }

            sessionManager.Add(ChatbotUserIdSessionKey, user.Id);

            var letstalkAccount = user.MessengerAccounts.FirstOrDefault(
                account => account.Type == MessengerType.Letstalk
                        && (account.Enabled || account.IsVerifying));

            if (letstalkAccount != null)
            {
                sessionManager.Add(LetstalkIdSessionKey, letstalkAccount.Username);
                sessionManager.Add(LetstalkAccountEnabledStatusKey, letstalkAccount.Enabled);
                sessionManager.Add(LetstalkAccountIsVerifyingStatusKey, letstalkAccount.IsVerifying);
            }
            else
            {
                sessionManager.Remove(LetstalkIdSessionKey);
                sessionManager.Remove(LetstalkAccountEnabledStatusKey);
                sessionManager.Remove(LetstalkAccountIsVerifyingStatusKey);
            }

            return new ChatBotViewModel(user);
        }

        public async Task<AddingUserResult> Handle(AddAccountCommand message)
        {
            var isNotAllowLinkAccount =
                sessionManager.Get<bool>(LetstalkAccountIsVerifyingStatusKey)
                || sessionManager.Get<bool>(LetstalkAccountEnabledStatusKey);

            if (isNotAllowLinkAccount)
            {
                return new AddingUserResult { Code = 1 };
            }

            var result = await settingService.AddUser(
                 message.CustId,
                 message.CustLevel,
                 message.SubAccountId,
                 message.Language,
                 message.LetstalkId,
                 MessengerType.Letstalk,
                 CreateLogParam(message));

            if (result.IsOk)
            {
                sessionManager.Add(ChatbotUserIdSessionKey, result.UserId);
                sessionManager.Add(LetstalkIdSessionKey, message.LetstalkId);
                sessionManager.Add(LetstalkAccountIsVerifyingStatusKey, true);
                sessionManager.Add(LetstalkAccountEnabledStatusKey, false);
            }

            return result;
        }

        public async Task<Result> Handle(UnsubscribeCommand message)
            => await settingService.Unsubscribe(
                  sessionManager.Get<int>(ChatbotUserIdSessionKey),
                  MessengerType.Letstalk,
                  sessionManager.Get<string>(LetstalkIdSessionKey),
                  CreateLogParam(message));

        public async Task<VerificationCodeResult> Handle(VerifyVerificationCodeCommand message)
        {
            if (sessionManager.Get<bool>(LetstalkAccountIsVerifyingStatusKey))
            {
                return await settingService.VerifyVerificationCode(
                    sessionManager.Get<int>(ChatbotUserIdSessionKey),
                    message.VerificationCode,
                    sessionManager.Get<string>(LetstalkIdSessionKey),
                    MessengerType.Letstalk,
                    CreateLogParam(message));
            }

            return new VerificationCodeResult { Code = 1 };
        }

        public async Task<Result> Handle(ResendVerificationCodeCommand message)
        {
            if (sessionManager.Get<bool>(LetstalkAccountIsVerifyingStatusKey))
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

            return new Result { Code = 1 };
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
                functions,
                CreateLogParam(message));
        }

        public async Task<Result> Handle(UpdateLanguageCommand message)
            => await settingService.UpdateLanguage(
                sessionManager.Get<int>(ChatbotUserIdSessionKey),
                message.Language,
                CreateLogParam(message));

        private static LogParam CreateLogParam(WriteLogCommandBase command)
            => new LogParam
            {
                ActorId = command.CustId,
                CustId = command.CustId,
                IP = command.IP
            };
    }
}