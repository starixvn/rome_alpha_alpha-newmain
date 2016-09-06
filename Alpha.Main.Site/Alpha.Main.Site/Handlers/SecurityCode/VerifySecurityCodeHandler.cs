namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using MediatR;
    using Models.Request.SecurityCode;
    using ViewModels.Security;
    public class VerifySecurityCodeHandler :
         IRequestHandler<VerifySecurityCodeQuery, VerifySecurityCodeViewModel>
    {
        public VerifySecurityCodeHandler()
        {
        }

        public VerifySecurityCodeViewModel Handle(VerifySecurityCodeQuery message)
        {
            return new VerifySecurityCodeViewModel();
        }
    }
}