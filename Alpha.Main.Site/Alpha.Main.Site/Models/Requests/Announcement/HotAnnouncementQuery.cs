namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using Dtos.Announcement;
    using MediatR;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        public string MessageIds { get; set; }

        public string AnnouncementResourcePath { get; set; }

        public string SuperPrivateMessagePath { get; set; }

        public string MasterPrivateMessagePath { get; set; }

        public string AgentPrivateMessagePath { get; set; }

        public string CommonAnnouncementPath { get; set; }
    }
}