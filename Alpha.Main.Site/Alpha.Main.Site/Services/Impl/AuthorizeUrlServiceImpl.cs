namespace Alpha.Main.Site.Services.Impl
{
    using Helpers;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizeUrlServiceImpl : AuthorizeUrlService
    {
        public bool CanAccessUrl(string url)
        {
            Argument.Requires(!string.IsNullOrWhiteSpace(url), nameof(url));

            var service = new AuthorizationBO(UserSession.Current.NetSessionId);
            return service.AuthorizeUrl(0, 0, false, url);
        }
    }
}