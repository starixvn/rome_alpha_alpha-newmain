namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Models.Announcements;
    using SecurityCodeCondition;

    public class DisableSecurityCodeAndValidAnnouncementTest
    {
        public HotAnnouncement Run()
        {
            // Arrange
            var securityCodeCondition = new DisableSecurityCodeCondition();
            var hotAnnouncementsParams = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(hotAnnouncementsParams);

            var validAnnouncementTimeCondition = new ValidAnnouncementTimeCondition();
            var query = hotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, validAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}
