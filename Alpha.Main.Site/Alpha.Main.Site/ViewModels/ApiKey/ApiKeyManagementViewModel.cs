namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using Alpha.Main.Site.Helpers;
    using System.Collections.Generic;

    public class ApiKeyManagementViewModel
    {
        public ApiKeyManagementViewModel(string custLevel, IEnumerable<ApiKeyInfo> apiKeyInfos, string language)
        {
            CustLevel = custLevel;
            ApiKeyInfos = apiKeyInfos;
            Language = language;
        }

        public string CustLevel { get; }

        public string Language { get; }

        public IEnumerable<ApiKeyInfo> ApiKeyInfos { get; }

        public string HelpPage => UrlHelper.MapMessagesPath("~/StaticMessage/Help?id=171247");

        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}