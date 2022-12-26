
namespace Alpha.Main.Models.ApiKey
{
    public class ApiKeyResult
    {
        public ApiKeyResult(string apiName, string key, string apiSecret)
        {
            ApiName = apiName;
            Key = key;
            ApiSecret = apiSecret;
        }

        public string ApiName { get; set; }

        public string Key { get; set; }

        public string ApiSecret { get; set; }
    }
}
