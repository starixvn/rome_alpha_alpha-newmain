namespace Alpha.Main.Site.Services.Impl
{
    using Helpers;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizeUrlServiceImpl : AuthorizeUrlService
    {
        private readonly AuthorizationBO authService;

        public AuthorizeUrlServiceImpl(AuthorizationBO authService = null)
        {
            this.authService = authService ?? new AuthorizationBO(UserSession.Current.NetSessionId);
        }

        public bool CanAccessUrl(string url)
        {
            Argument.Requires(!string.IsNullOrWhiteSpace(url), nameof(url));
            
            return authService.AuthorizeUrl(0, 0, false, url);
        }
    }
}