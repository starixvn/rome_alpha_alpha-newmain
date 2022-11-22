namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;

    public class UpdateApiKeyCommand : IAsyncRequest<Result>
    {
        public UpdateApiKeyCommand() : this(null)
        {
        }

        public UpdateApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
        }

        public string ApiKey { get; set; }

        public int CustId { get; }

        public string ApiName { get; set; }

        public string Scopes { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}