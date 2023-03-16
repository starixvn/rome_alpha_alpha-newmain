namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Helpers;
    using System;
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

        public DateTime CurrentDate => DateTime.Now;

        public string HelpPage => UrlHelper.MapMessagesPath("~/StaticMessage/Help?id=171247");

        public string APIGuidesURL => UrlHelper.MapMessagesPath("~/StaticMessage/Help?id=171248");

        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}