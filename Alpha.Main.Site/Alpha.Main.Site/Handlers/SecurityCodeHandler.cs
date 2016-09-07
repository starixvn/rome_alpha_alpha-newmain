namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using Alpha.SecurityCode.Configurations;
    using Alpha.SecurityCode.Entities;
    using Alpha.SecurityCode.Sessions;
    using AppResources;
    using Configurations;
    using DataAccesses;
    using Fanex.Security;
    using Main.Models.SecurityCode;
    using MediatR;
    using Models.Request.SecurityCode;
    using Services;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ViewModels.Security;


    public class SecurityCodeHandler :
         IRequestHandler<SecurityCodeQuery, SecurityCodeViewModel>
    {
        public SecurityCodeViewModel Handle(SecurityCodeQuery message)
        {
            return new SecurityCodeViewModel();
        }
    }
}