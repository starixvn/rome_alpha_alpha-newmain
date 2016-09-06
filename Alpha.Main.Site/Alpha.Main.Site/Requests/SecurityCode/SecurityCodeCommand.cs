namespace Alpha.Main.Site.Models.SecurityCode
{
    using MediatR;
    using ViewModels;

    public class SecurityCodeCommand : IRequest<HeaderNavViewModel>
    {
        public int CustId { get; set; }
    }
}