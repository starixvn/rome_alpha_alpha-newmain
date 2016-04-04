namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;
    using ViewModels;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
    }
}