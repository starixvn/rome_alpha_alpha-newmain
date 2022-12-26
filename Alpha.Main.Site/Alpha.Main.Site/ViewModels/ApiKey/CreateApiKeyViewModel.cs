
namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CreateApiKeyViewModel
    {
        //public CreateApiKeyViewModel(ApiKeyResult apiKeyResult)
        //{
        //    ApiKeyResult = apiKeyResult;
        //}

        //public ApiKeyResult ApiKeyResult { get; set; }

        public CreateApiKeyViewModel()
        {

        }

        public List<string> Scopes { get; set; }
    }
}