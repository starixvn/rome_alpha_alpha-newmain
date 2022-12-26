
namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Extensions;
    using Alpha.Main.Models.ApiKey;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiKeyServiceImpl : ApiKeyService
    {
        private readonly HttpClient httpClient;

        public ApiKeyServiceImpl(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiKeyResult> CreateApiKey(int custId, string apiName, string passPhrase, string scopes, DateTime expirationDate)
        {
            var parameter = new { custId, apiName, passPhrase, scopes, expirationDate };
            return await httpClient.PostAsJsonAsync<ApiKeyResult>(string.Empty, parameter);
        }

        public async Task<IEnumerable<ApiKeyInfo>> GetApiKeyByCustId(int custId)
        {
            return await httpClient.GetAsJsonAsync<IEnumerable<ApiKeyInfo>>(custId.ToString());
        }

        public async Task<ApiKeyInfo> GetApiKeyByKey(string key)
        {
            return await httpClient.GetAsJsonAsync<ApiKeyInfo>(key);
        }

        public async Task RevokeApiKey(string apiKey)
        {
            await httpClient.DeleteAsStringAsync(apiKey);
        }

        public async Task UpdateApiKey(string apiKey, int custId, string apiName, string scopes, DateTime expirationDate)
        {
            var parameter = new { apiKey, custId, apiName, scopes, expirationDate };
            await httpClient.PutAsStringAsync(string.Empty, parameter);
        }
    }
}
