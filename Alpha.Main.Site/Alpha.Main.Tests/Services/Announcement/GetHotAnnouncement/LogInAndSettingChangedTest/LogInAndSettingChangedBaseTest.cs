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
        }

        public GetHotAnnouncementsParams HotAnnouncementsParams { get; set; }
    }
}
