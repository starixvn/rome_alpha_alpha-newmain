namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using System.Collections.Generic;

    public class ApiKeyManagementViewModel
    {
        public ApiKeyManagementViewModel(string custLevel, IEnumerable<ApiKeyInfo> apiKeyInfos)
        {
            CustLevel = custLevel;
            ApiKeyInfos = apiKeyInfos;
        }

        public string CustLevel { get; }

        public IEnumerable<ApiKeyInfo> ApiKeyInfos { get; }
    }
}