namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using System;
    using MediatR;
    using Models.Request.SecurityCode;
    using ViewModels.Security;
    public class ForceChangeSecurityCodeWithoutValidationQueryHandler :
         IRequestHandler<ForceChangeSecurityCodeWithoutValidationQuery, ForceChangeSecurityCodeWithoutValidationViewModel>
    {
        public ForceChangeSecurityCodeWithoutValidationQueryHandler()
        {
        }

        public ForceChangeSecurityCodeWithoutValidationViewModel Handle(ForceChangeSecurityCodeWithoutValidationQuery message)
        {
            return new ForceChangeSecurityCodeWithoutValidationViewModel();
        }
    }
}