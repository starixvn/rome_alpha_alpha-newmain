namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using MediatR;
    using Models.Request.SecurityCode;
    using ViewModels.Security;
    public class ForceChangeSecurityCodeHandler :
         IRequestHandler<ForceChangeSecurityCodeQuery, ForceChangeSecurityCodeViewModel>
    {
        public ForceChangeSecurityCodeHandler()
        {
        }

        public ForceChangeSecurityCodeViewModel Handle(ForceChangeSecurityCodeQuery message)
        {
            return new ForceChangeSecurityCodeViewModel();
        }
    }
}