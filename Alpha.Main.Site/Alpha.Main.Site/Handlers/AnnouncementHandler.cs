namespace Alpha.Main.Site.Handlers
{
    using System;
    using Main.Models.Announcements;
    using MediatR;
    using Models.Requests.Announcement;
    using Params.Announcements;
    using Services;
    using Sunplus.Agent.Utilities;

    public class AnnouncementHandler :
         IRequestHandler<HotAnnouncementQuery, HotAnnouncement>
    {
        private readonly AnnouncementService _announcementService;

        public AnnouncementHandler(
            AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public HotAnnouncement Handle(HotAnnouncementQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var hotAnnouncementsParams = new GetHotAnnouncementsParams();
            message.AssignValue(hotAnnouncementsParams);

            var hotAnnouncement = _announcementService.GetHotAnnouncement(hotAnnouncementsParams);

            hotAnnouncement.CookieId = Encryption.PearsonEncode(message.UserId.ToString() + message.SubAccId.ToString());

            return hotAnnouncement;
        }
    }
}