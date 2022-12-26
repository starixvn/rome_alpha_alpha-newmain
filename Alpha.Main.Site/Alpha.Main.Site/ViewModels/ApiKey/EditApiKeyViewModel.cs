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
        public EditApiKeyViewModel(ApiKeyInfo apiKeyInfo)
        {
            ApiKeyInfo = apiKeyInfo;
        }

        public ApiKeyInfo ApiKeyInfo { get; set; }

        public List<string> Scopes { get; set; }
    }
}