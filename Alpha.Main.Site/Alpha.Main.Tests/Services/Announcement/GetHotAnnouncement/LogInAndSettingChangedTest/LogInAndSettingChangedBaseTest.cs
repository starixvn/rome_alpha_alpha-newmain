namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest
{
    using Alpha.Main.Params.Announcements;
    using SecurityCodeCondition;

    public class LogInAndSettingChangedBaseTest
    {
        public LogInAndSettingChangedBaseTest()
        {
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            HotAnnouncementsParams = securityCodeCondition.HotAnnouncementsParams();

            ValidAnnouncementTimeCondition = new ValidAnnouncementTimeCondition();
        }

        public GetHotAnnouncementsParams HotAnnouncementsParams { get; set; }

        public ValidAnnouncementTimeCondition ValidAnnouncementTimeCondition { get; set; }
    }
}
