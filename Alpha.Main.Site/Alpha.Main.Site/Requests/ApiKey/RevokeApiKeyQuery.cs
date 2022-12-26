
namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class RevokeApiKeyQuery : IAsyncRequest<Result>
    {
        public RevokeApiKeyQuery() : this(null)
        {
        }

        public RevokeApiKeyQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
        }

        public string Key { get; set; }
    }
}