namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Constants;
    using System.Collections.Generic;

    public class CreateApiKeyViewModel
    {
        public CreateApiKeyViewModel(string custLevel)
        {
            CustLevel = custLevel;
        }

        public string CustLevel { get; }

        public List<string> Scopes = new List<string>
        {
            ApiKeyScope.WinLossDetail,
            ApiKeyScope.Outstanding
        };
    }
}