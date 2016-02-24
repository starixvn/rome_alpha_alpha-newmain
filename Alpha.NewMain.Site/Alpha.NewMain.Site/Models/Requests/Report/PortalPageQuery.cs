namespace Alpha.NewMain.Site.Models.Requests.Report
{
    using Alpha.NewMain.Site.Models.Views.Report;
    using MediatR;
    public class PortalPageQuery : IRequest<PortalPageViewModel>
    {
        public string LastPostTime { get; set; }
    }
}