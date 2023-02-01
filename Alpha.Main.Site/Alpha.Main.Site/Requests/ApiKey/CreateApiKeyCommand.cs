namespace Alpha.Main.Site.Requests.ApiKey
{
    using MediatR;
    using System;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Alpha.Main.Models.ApiKey;

    public class CreateApiKeyCommand : IAsyncRequest<CreateApiKeyResult>
    {
        public CreateApiKeyCommand() : this(null)
        {
        }

        public CreateApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            IP = userSession.UserLoginIp;
        }

        public int CustId { get; }

        public string ApiName { get; set; }

        public string PassPhrase { get; set; }

        public string Scopes { get; set; }

        public string IP { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}