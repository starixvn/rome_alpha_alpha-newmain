namespace Alpha.NewMain.Site.Models.Requests.Report
{
    using MediatR;
    using Views.Report;

    public class MoreAnnouncementsQuery : IRequest<AnnouncementViewModel>
    {
        public string LastPostTime { get; set; }
    }
}