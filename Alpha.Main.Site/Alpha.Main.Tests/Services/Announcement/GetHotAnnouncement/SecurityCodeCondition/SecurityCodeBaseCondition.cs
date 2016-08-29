namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using Params.Announcements;
    using SecurityCode.Sessions;
    public class SecurityCodeBaseCondition
    {
        public ISecurityCodeSession SecurityCodeSession { get; set; }

        public bool IsSecurityCodeActive { get; set; }

        public GetHotAnnouncementsParams HotAnnouncementsParams()
        {
            var query = new GetHotAnnouncementsParams(SecurityCodeSession, IsSecurityCodeActive);
            return query;
        }
    }
}
