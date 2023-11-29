namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using AlphaPlus.User.Models;
    using MediatR;

    public class RevokeApiKeyCommand : IAsyncRequest<Result>
    {
        public RevokeApiKeyCommand() : this(null)
        {
        }

        public RevokeApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            IP = userSession.UserLoginIp;
        }

        public string Key { get; set; }

        public string IP { get; }
    }
}