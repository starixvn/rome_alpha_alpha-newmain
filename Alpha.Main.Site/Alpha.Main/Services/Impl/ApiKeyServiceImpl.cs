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

        public async Task<CreateApiKeyResult> CreateApiKey(int custId, string apiName, string passPhrase, string scopes, string ip, DateTime? expirationDate)
        {
            var parameter = new { custId, apiName, passPhrase, scopes, ip, expirationDate };
            return await httpClient.PostAsJsonAsync<CreateApiKeyResult>(string.Empty, parameter);
        }

        public async Task<IEnumerable<ApiKeyInfo>> GetApiKeyByCustId(int custId)
        {
            return await httpClient.GetAsJsonAsync<IEnumerable<ApiKeyInfo>>(custId.ToString());
        }

        public async Task<ApiKeyInfo> GetApiKeyByKey(string key)
        {
            return await httpClient.GetAsJsonAsync<ApiKeyInfo>(key);
        }

        public async Task RevokeApiKey(string apiKey, string ip)
        {
            await httpClient.DeleteAsStringAsync($"{apiKey}?ip={ip}");
        }

        public async Task<Result> UpdateApiKey(string apiKey, int custId, string apiName, string scopes, string ip, DateTime? expirationDate)
        {
            var parameter = new { apiKey, custId, apiName, scopes, ip, expirationDate };
            return await httpClient.PutAsJsonAsync<Result>(string.Empty, parameter);
        }
    }
}
