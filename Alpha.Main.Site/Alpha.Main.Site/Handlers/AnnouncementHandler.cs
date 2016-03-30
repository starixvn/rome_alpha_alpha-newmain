namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class AnnouncementHandler :
         IRequestHandler<HotAnnouncementQuery, HotAnnouncement>
    {
        private readonly IAnnouncementService _announcementService;
        private readonly IUserSession _userSession;

        public AnnouncementHandler(IAnnouncementService announcementService, IUserSession userSession)
        {
            _announcementService = announcementService;
            _userSession = userSession;
        }

        public HotAnnouncement Handle(HotAnnouncementQuery message)
        {
            var hotAnnouncement = _announcementService.GetHotAnnouncement(message);
            hotAnnouncement.CookieId = Encryption.PearsonEncode(_userSession.UserId.ToString() + _userSession.SubAccId.ToString());

            return hotAnnouncement;
        }
    }
}