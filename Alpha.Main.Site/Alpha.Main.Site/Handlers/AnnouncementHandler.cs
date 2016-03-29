namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;
    using Services;

    public class AnnouncementHandler :
         IRequestHandler<HotAnnouncementQuery, HotAnnouncement>
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementHandler(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public HotAnnouncement Handle(HotAnnouncementQuery message)
        {
            return _announcementService.GetHotAnnouncement(message);
        }
    }
}