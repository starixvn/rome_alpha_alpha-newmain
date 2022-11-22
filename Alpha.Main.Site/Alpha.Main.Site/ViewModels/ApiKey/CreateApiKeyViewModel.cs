namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Constants;
    using System.Collections.Generic;

    public class CreateApiKeyViewModel
    {
        public CreateApiKeyViewModel(string custLevel, string language)
        {
            CustLevel = custLevel;
            Language = language;
        }

        public string CustLevel { get; }

        public string Language { get; }

        public List<string> Scopes = new List<string>
        {
            ApiKeyScope.WinLossDetail,
            ApiKeyScope.Outstanding
        };
    }
}