namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using System.Web;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using AppResources;
    using Fanex.Common;
    using Main.Models.CustomerProfiles;
    using MediatR;
    using Services;
    using ViewModels.CustomerProfile;
    using Models.Request.SecurityCode;
    using ViewModels;
    public class SecurityCodeHandler :
         IRequestHandler<SecurityCodeQuery, SecurityModel>
    {
        private readonly SiteMenuService siteMenuService;

        public SecurityCodeHandler(
            SiteMenuService siteMenuService)
        {
            this.siteMenuService = siteMenuService;
        }

        public SecurityModel Handle(SecurityCodeQuery message)
        {
            return new SecurityModel();
        }
}
}