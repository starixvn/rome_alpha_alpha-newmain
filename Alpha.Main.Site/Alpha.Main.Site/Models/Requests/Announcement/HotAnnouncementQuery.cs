namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using Dtos.Announcement;
    using MediatR;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        public string MessageIds { get; set; }
    }
}