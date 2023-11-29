namespace Alpha.Main.Site.Requests.ApiKey
{
    using System;
    using Alpha.Main.Models.ApiKey;
    using AlphaPlus.User.Models;
    using MediatR;

    public class UpdateApiKeyCommand : IAsyncRequest<Result>
    {
        public UpdateApiKeyCommand() : this(null)
        {
        }

        public UpdateApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            IP = userSession.UserLoginIp;
            Language = userSession.LangKey;
        }

        public string ApiKey { get; set; }

        public int CustId { get; }

        public string ApiName { get; set; }

        public string Scopes { get; set; }

        public string IP { get; }

        public string Language { get; }

        public DateTime? ExpirationDate { get; set; }
    }
}