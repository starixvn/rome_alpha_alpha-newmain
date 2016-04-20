namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.AgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using Models.Announcements;

    public class SettingChangedTest : LogInAndSettingChangedBaseTest
    {
        public HotAnnouncement Run(
            bool isPositionTakingChanged,
            bool isBettingLimitChanged,
            bool isInsertPersonalMessage,
            string expectedAnnouncementPrefix)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var logInAndSettingChangedCondition = new AgentDirectLoginAndSettingChangedCondition(isPositionTakingChanged, isBettingLimitChanged, isInsertPersonalMessage);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(logInAndSettingChangedCondition.DynamicRepository);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}
