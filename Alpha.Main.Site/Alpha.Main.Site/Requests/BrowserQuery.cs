namespace Alpha.Main.Site.Requests
{
    using Alpha.Main.Site.ViewModels;
    using Main.Models.Enumerations;
    using MediatR;

    public class BrowserQuery : IRequest<BrowserViewModel>
    {
        public byte UserLevelId { get; set; }
    }
}