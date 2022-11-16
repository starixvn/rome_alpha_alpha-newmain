namespace Alpha.Main.Services
{
    using Alpha.Main.Models.ApiKey;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface ApiKeyService
    {
        Task<ApiKeyResult> CreateApiKey(int custId, string apiName, string passPhrase, string scopes, DateTime? expirationDate);

        Task RevokeApiKey(string apiKey);

        Task UpdateApiKey(string apiKey, int custId, string apiName, string scopes, DateTime? expirationDate);

        Task<IEnumerable<ApiKeyInfo>> GetApiKeyByCustId(int custId);

        Task<ApiKeyInfo> GetApiKeyByKey(string key);
    }
}
