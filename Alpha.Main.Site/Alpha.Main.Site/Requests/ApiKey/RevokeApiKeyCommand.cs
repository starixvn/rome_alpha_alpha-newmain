namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class RevokeApiKeyCommand : IAsyncRequest<Result>
    {
        public RevokeApiKeyCommand() : this(null)
        {
        }

        public RevokeApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
        }

        public string Key { get; set; }
    }
}