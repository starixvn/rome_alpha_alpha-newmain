namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using Params.Announcements;
    using Sessions;

    public class SecurityCodeBaseCondition
    {
        public SecurityCodeSession SecurityCodeSession { get; set; }

        public bool IsSecurityCodeActive { get; set; }

        public GetHotAnnouncementsParams HotAnnouncementsParams()
        {
            var query = new GetHotAnnouncementsParams();
            return query;
        }
    }
}