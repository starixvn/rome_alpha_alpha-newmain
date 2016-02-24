namespace Alpha.NewMain.Site.Handlers
{
    using MediatR;
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;
    using Services;

    public class AnnouncementHandler :
         IRequestHandler<HotAnnouncementQuery, HotAnnouncementModel>
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementHandler(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public HotAnnouncementModel Handle(HotAnnouncementQuery message)
        {
            return _announcementService.GetHotAnnouncement(message);
        }
    }
}