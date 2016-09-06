namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using MediatR;
    using Models.Request.SecurityCode;
    using ViewModels.Security;
    public class ChangeSecurityCodeHandler :
         IRequestHandler<ChangeSecurityCodeQuery, ChangeSecurityCodeViewModel>
    {
        public ChangeSecurityCodeHandler()
        {
        }

        public ChangeSecurityCodeViewModel Handle(ChangeSecurityCodeQuery message)
        {
            return new ChangeSecurityCodeViewModel();
        }
    }
}