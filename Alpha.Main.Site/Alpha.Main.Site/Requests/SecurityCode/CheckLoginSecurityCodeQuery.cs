namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;

    public class CheckLoginSecurityCodeQuery : IRequest<SecurityCodeStatus>
    {
    }
}