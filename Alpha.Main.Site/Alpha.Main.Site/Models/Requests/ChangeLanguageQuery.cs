namespace Alpha.Main.Site.Models.Requests
{
    using Dtos;
    using MediatR;

    public class ChangeLanguageQuery : IRequest<ResultModel>
    {
        public string Language { get; set; }
    }
}