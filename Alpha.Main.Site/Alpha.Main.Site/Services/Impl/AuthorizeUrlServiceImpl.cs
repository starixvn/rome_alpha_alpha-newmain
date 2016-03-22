namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizeUrlServiceImpl : AuthorizeUrlService
    {
        public bool CanAccessUrl(string url)
        {
            Contract.Ensures(string.IsNullOrWhiteSpace(url), nameof(url));

            AuthorizationBO service = new AuthorizationBO(UserSession.Current.NetSessionId);
            return service.AuthorizeUrl(0, 0, false, url);            
        }
    }
}