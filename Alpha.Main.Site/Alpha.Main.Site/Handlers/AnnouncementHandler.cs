namespace Alpha.Main.Site.Handlers
{
    using Main.Models.Announcements;
    using MediatR;
    using Models.Requests.Announcement;
    using Params.Announcements;
    using SecurityCode.Sessions;
    using Services;
    using Sunplus.Agent.Utilities;
    using System;

    public class AnnouncementHandler :
         IRequestHandler<HotAnnouncementQuery, HotAnnouncement>
    {
        private readonly AnnouncementService _announcementService;
        private readonly ISecurityCodeSession securityCodeSession;

        public AnnouncementHandler(
            AnnouncementService announcementService,
            ISecurityCodeSession securityCodeSession = null)
        {
            _announcementService = announcementService;
            this.securityCodeSession = securityCodeSession ?? CookieLessSessionFactory.SecurityCodeSession;
        }

        public HotAnnouncement Handle(HotAnnouncementQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var hotAnnouncementsParams = new GetHotAnnouncementsParams(securityCodeSession, message.IsSecurityCodeActive);
            message.AssignValue(hotAnnouncementsParams);

            var hotAnnouncement = _announcementService.GetHotAnnouncement(hotAnnouncementsParams);

            hotAnnouncement.CookieId = Encryption.PearsonEncode(message.UserId.ToString() + message.SubAccId.ToString());

            return hotAnnouncement;
        }
    }
}