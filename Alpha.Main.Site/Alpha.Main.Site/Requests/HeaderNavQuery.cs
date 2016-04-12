namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;
    using ViewModels;

    public class HeaderNavQuery : IRequest<HeaderNavViewModel>
    {
        public int CustId { get; set; }
    }
}