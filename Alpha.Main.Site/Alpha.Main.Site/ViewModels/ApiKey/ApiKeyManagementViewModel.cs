namespace Alpha.Main.Site.ViewModels.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ApiKeyManagementViewModel
    {
        public IEnumerable<ApiKeyInfo> ApiKeyInfos { get; set; }
    }
}