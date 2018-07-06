using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Client.Models;
using Chatbot.Client.Services;
using Fanex.Caching;

namespace Alpha.Main.Site.DummyService
{
    public class SettingServiceImpl : SettingService
    {
        private const string ChatBotUserKey = "ChatBot.Client.User";
        private readonly List<User> Users;

        public SettingServiceImpl()
        {
            //CacheService.Remove(ChatBotUserKey);
            var cachedUsers = CacheService.Get<List<User>>(ChatBotUserKey);
            Users = cachedUsers ?? new List<User>();
        }

        public Task<AddingUserResult> AddUser(int custId, int subAccountId, string language, string messengerAccount, int messengerType = 1)
        {
            var mesenger = new Messenger
            {
                Account = messengerAccount,
                Enabled = true,
                Type = 1,
                VerificationStatus = VerificationStatusCode.Done
            };

            var functions = new List<Function>
           {
               new Function { Id=1, Name="Outstanding", Command="Outstanding", Enabled=true, Type=1 },
               new Function { Id=1,Name="Notification", Command="", Enabled=true, Type=2 }
           };

            var user = new User
            {
                Id = Users.Count + 1,
                CustId = custId,
                SubAccountId = subAccountId,
                Language = language,
                MessengerAccounts = new List<Messenger> { mesenger },
                Functions = functions
            };

            Users.Add(user);
            CacheService.Set(ChatBotUserKey, Users);

            return Task.FromResult(new AddingUserResult { UserId = user.Id });

            //return Task.FromResult(AddingUserResult.CreateAccountDoesNotExistResult());
        }

        public Task<Result> GenerateVerificationCode(int userId, string messengerAccount, int messengerType = 1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Function>> GetFunctions(int userId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int custId, int subAccountId, string language)
        => Task.FromResult(Users.FirstOrDefault(u => (u.CustId == custId || u.SubAccountId == subAccountId)));

        public Task<Result> Unsubscribe(int userId, int messengerType)
        {
            Users.RemoveAll(u => u.Id == userId);
            CacheService.Set(ChatBotUserKey, Users);

            return Task.FromResult(new Result { Code = 0 });
        }

        public Task<Result> UpdateFunctions(int userId, IEnumerable<Function> functions)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateLanguage(int userId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<VerificationCodeResult> VerifyVerificationCode(int userId, string verificationCode, string messengerAccount, int messengerType = 1)
        {
            return Task.FromResult(new VerificationCodeResult { Code = 4, FailedTimes = 3 });
        }
    }
}