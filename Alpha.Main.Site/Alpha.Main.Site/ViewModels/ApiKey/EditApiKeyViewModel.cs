namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Constants;
    using Alpha.Main.Models.ApiKey;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class EditApiKeyViewModel
    {
        public EditApiKeyViewModel(ApiKeyInfo apiKeyInfo, List<string> scopes, string language)
        {
            ApiKeyInfo = apiKeyInfo;
            Scopes = scopes;
            Language = language;
        }

        public ApiKeyInfo ApiKeyInfo { get; }

        public List<string> Scopes { get; }

        public string Language { get; }
    }
}