using Alpha.Main.AppResources;
using Alpha.Main.Configurations;
using Alpha.Main.Constants;
using Alpha.Main.Criteria.Customer;
using Fanex.Data.Repository;
using Newtonsoft.Json;
using NovaCasino.API.Client.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Main.Services
{
    public interface CustomerService
    {
        Task<IEnumerable<User>> GetUplines(int custId);

        Task UpdateSyncedNovaCasino(string[] custIds, string ip);

        Task InsertPersonalMessage(UserInfo userInfo);
    }

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Task<IEnumerable<User>> GetUplines(int custId)
            => dynamicRepository.FetchAsync<User>(new GetUplineInfoCriteria(custId));

        public Task InsertPersonalMessage(UserInfo userInfo)
        {
            var codeKeys = AppSettings.DisabledVietnamese ? Language.CodeNamesWithoutVietNamese : Language.CodeKeys;
            var personalMessages = codeKeys.ToDictionary(p => p.Key, p => string.Format(GetMessageTemplate(p.Key), userInfo.Username));
            var command = new InsertPersonalMessageCommand(
                JsonConvert.SerializeObject(userInfo.Upline),
                personalMessages[Language.English],
                personalMessages[Language.ChineseSimplified],
                personalMessages[Language.ChineseTraditional],
                personalMessages[Language.Thai],
                personalMessages.ContainsKey(Language.Vietnamese) ? personalMessages[Language.Vietnamese] : null);
            return dynamicRepository.ExecuteAsync(command);
        }

        public Task UpdateSyncedNovaCasino(string[] custIds, string ip)
        {
            var tasks = new List<Task>();
            foreach (var item in custIds)
            {
                int custId;
                if (int.TryParse(item, out custId))
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var command = new UpdateSyncedNovaCasinoCommand(custId, ip);
                        return dynamicRepository.ExecuteAsync(command);
                    }));
                }
            }

            return Task.WhenAll(tasks);
        }

        private static string GetMessageTemplate(string languageCode)
            => Resources.ResourceManager.GetString(nameof(Resources.SyncedNovaCasinoPersonalMessage), new CultureInfo(languageCode));
    }
}