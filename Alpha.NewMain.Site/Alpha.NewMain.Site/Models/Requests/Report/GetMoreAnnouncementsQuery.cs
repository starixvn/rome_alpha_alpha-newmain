namespace Alpha.NewMain.Site.Models.Requests.Report
{
    using MediatR;
    using Views.Report;

    public class GetMoreAnnouncementsQuery : IRequest<AnnouncementViewModel>
    {
        public string LastPostTime { get; set; }
    }
}