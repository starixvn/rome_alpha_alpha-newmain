namespace Alpha.Main.Services
{
    using Alpha.Main.Models.ApiKey;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface ApiKeyService
    {
        Task<CreateApiKeyResult> CreateApiKey(int custId, string apiName, string passPhrase, string scopes, string ip, string language, DateTime? expirationDate);

        Task RevokeApiKey(string apiKey, string ip);

        Task<Result> UpdateApiKey(string apiKey, int custId, string apiName, string scopes, string ip, string language, DateTime? expirationDate);

        Task<IEnumerable<ApiKeyInfo>> GetApiKeyByCustId(int custId);

        Task<ApiKeyInfo> GetApiKeyByKey(string key);
    }
}
