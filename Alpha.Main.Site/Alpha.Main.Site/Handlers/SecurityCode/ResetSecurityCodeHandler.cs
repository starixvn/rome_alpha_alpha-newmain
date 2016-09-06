namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using MediatR;
    using Models.Request.SecurityCode;
    using ViewModels.Security;
    public class ResetSecurityCodeHandler :
         IRequestHandler<ResetSecurityCodeQuery, ResetSecurityCodeViewModel>
    {
        public ResetSecurityCodeHandler()
        {
        }

        public ResetSecurityCodeViewModel Handle(ResetSecurityCodeQuery message)
        {
            return new ResetSecurityCodeViewModel();
        }
    }
}