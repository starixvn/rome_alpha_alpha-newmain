namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Site.ViewModels.ApiKey;
    using AlphaPlus.User.Models;
    using MediatR;

    public class GetApiKeyQuery : IAsyncRequest<EditApiKeyViewModel>
    {
        public GetApiKeyQuery() : this(null)
        {
        }

        public GetApiKeyQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            Language = userSession.Language;
        }

        public string Language { get; }

        public string Key { get; set; }
    }
}