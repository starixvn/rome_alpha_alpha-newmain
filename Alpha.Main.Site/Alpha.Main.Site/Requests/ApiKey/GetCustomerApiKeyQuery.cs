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

    public class GetCustomerApiKeyQuery : IAsyncRequest<ApiKeyManagementViewModel>
    {
        public GetCustomerApiKeyQuery() : this(null)
        {
        }

        public GetCustomerApiKeyQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = 1176086;
        }

        public int CustId { get; set; }
    }
}