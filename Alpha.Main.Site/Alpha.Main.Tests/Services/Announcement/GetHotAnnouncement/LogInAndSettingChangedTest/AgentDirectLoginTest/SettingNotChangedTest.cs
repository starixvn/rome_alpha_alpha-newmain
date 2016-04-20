namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.AgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Models.Announcements;

    public class SettingNotChangedTest : LogInAndSettingChangedBaseTest
    {
        public HotAnnouncement Run(bool hasAnnouncement, string expectedAnnouncementContent)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var logInAndSettingChangedCondition = new AgentDirectLoginAndSettingChangedCondition(false, false);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(
                logInAndSettingChangedCondition.DynamicRepository,
                showedMessageCondition.SecurityCodeService);
            var localFileAnnouncementCondition = LocalFileConditionFactory.Create(hasAnnouncement);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}
