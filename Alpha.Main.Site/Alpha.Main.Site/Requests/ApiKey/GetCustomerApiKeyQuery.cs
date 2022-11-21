namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Site.ViewModels.ApiKey;
    using Alpha.Web.Models;
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
            CustId = userSession.CustId;
            CustLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }
    }
}