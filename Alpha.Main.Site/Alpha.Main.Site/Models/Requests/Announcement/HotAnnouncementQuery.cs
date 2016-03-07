namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using Dtos.Announcement;
    using MediatR;

    public class HotAnnouncementQuery : IRequest<HotAnnouncementModel>
    {
        public string MessageIds { get; set; }
    }
}