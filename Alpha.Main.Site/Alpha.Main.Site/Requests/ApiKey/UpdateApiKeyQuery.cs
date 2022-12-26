
namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class UpdateApiKeyQuery : IAsyncRequest<Result>
    {
        public UpdateApiKeyQuery() : this(null)
        {
        }

        public UpdateApiKeyQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = 1176086;
        }

        public string Key { get; set; }

        public int CustId { get; set; }

        public string ApiName { get; set; }

        public string Scopes { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}