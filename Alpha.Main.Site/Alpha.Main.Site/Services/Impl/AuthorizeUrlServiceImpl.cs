namespace Alpha.Main.Site.Services.Impl
{
    using Helpers;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizeUrlServiceImpl : AuthorizeUrlService
    {
        private AuthorizationBO authService;

        private AuthorizationBO AuthorizeServie
        {
            get
            {
                if (authService == null)
                {
                    return new AuthorizationBO(UserSession.Current.NetSessionId);
                }

                return authService;
            }            
        }

        public bool CanAccessUrl(string url)
        {
            Argument.Requires(!string.IsNullOrWhiteSpace(url), nameof(url));
            
            return AuthorizeServie.AuthorizeUrl(0, 0, false, url);
        }
    }
}