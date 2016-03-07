namespace Alpha.Main.Site.Models.Requests
{
    using Alpha.Main.Site.Models.Views;
    using MediatR;

    public class MainIndexQuery : IRequest<MainIndexViewModel>
    {
    }
}