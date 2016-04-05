namespace Alpha.Main.Site.Models.Requests
{
    using Alpha.Main.Site.ViewModels;
    using MediatR;

    public class EmptyQuery : 
        IRequest<IndexViewModel>, 
        IRequest<LanguageViewModel>,
        IRequest<HeaderNavViewModel>
    {
    }
}