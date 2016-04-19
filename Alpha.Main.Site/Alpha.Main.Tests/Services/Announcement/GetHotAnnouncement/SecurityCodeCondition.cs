namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Fanex.Razorbill.Security.Sessions;
    using Params.Announcements;

    public class SecurityCodeCondition
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
