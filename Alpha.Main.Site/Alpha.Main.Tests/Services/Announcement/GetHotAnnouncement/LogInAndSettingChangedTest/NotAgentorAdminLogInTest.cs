namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Models.Announcements;

    public class NotAgentOrAdminLogInTest : LogInAndSettingChangedBaseTest
    {
        public HotAnnouncement Run(
            byte roleId,
            bool isAdminLogIn,
            bool hasAnnouncement,
            string expectedAnnouncement)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition(roleId, isAdminLogIn);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService);

            var localFileAnnouncementCondition = LocalFileConditionFactory.Create(hasAnnouncement);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}
