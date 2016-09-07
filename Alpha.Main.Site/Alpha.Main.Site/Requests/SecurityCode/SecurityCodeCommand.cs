namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Alpha.SecurityCode.Sessions;
    using Main.Models.SecurityCode;
    using MediatR;
    using System.Configuration;
    using ViewModels.Security;
    public class SecurityCodeCommand : IRequest<Result>
    {
       
    }
}