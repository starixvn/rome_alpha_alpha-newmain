namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using Params.Announcements;

    public class SecurityCodeBaseCondition
    {
        public bool IsSecurityCodeActive { get; set; }

        public GetHotAnnouncementsParams HotAnnouncementsParams()
        {
            var query = new GetHotAnnouncementsParams();
            query.NoticeTypes = new[] { "1_2", "1", "2", "3" };
            return query;
        }
    }
}