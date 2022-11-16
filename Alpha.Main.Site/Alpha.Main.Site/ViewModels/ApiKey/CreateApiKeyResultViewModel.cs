namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CreateApiKeyResultViewModel
    {
        public string ApiName { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }
    }
}