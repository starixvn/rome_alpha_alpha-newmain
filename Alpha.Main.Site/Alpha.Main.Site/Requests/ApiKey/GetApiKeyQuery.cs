namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Site.ViewModels.ApiKey;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class GetApiKeyQuery : IAsyncRequest<EditApiKeyViewModel>
    {
        public GetApiKeyQuery() : this(null)
        {
        }

        public GetApiKeyQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
        }

        public string Key { get; set; }
    }
}