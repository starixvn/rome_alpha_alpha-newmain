namespace Alpha.Main.Site.Handlers
{
    using Main.Models.Announcements;
    using MediatR;
    using Models.Requests.Announcement;
    using Params.Announcements;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class AnnouncementHandler :
         IRequestHandler<HotAnnouncementQuery, HotAnnouncement>
    {
        private readonly AnnouncementService _announcementService;

        public AnnouncementHandler(AnnouncementService announcementService, IUserSession userSession)
        {
            _announcementService = announcementService;
        }

        public HotAnnouncement Handle(HotAnnouncementQuery message)
        {
            var hotAnnouncement = _announcementService.GetHotAnnouncement(new GetHotAnnouncementsParams()
            {
                MessageIds = message.MessageIds,
                PrivateMessagePath = message.PrivateMessagePath,
                CommonAnnouncementPath = message.CommonAnnouncementPath,
                AdminId = message.AdminId,
                CustId = message.CustId,
                UserId = message.UserId,
                Language = message.Language,
                ProductsStatus = message.ProductsStatus,
                RoleId = message.RoleId,
                Site = message.Site,
                SubAccId = message.SubAccId,
                UserName = message.UserName
            });

            hotAnnouncement.CookieId = Encryption.PearsonEncode(message.UserId.ToString() + message.SubAccId.ToString());

            return hotAnnouncement;
        }
    }
}