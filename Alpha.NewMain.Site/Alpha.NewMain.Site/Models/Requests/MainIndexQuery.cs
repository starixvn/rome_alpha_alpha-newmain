namespace Alpha.NewMain.Site.Models.Requests
{
    using Alpha.NewMain.Site.Models.Views;
    using MediatR;

    public class MainIndexQuery : IRequest<MainIndexViewModel>
    {
    }
}