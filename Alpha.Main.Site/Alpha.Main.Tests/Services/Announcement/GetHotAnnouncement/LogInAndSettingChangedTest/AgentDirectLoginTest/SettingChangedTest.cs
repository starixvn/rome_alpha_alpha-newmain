namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.AgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using NUnit.Framework;

    public class SettingChangedTest : LogInAndSettingChangedBaseTest
    {
        public void Run(
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

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement);
            Assert.AreEqual(true, actualHotAnnouncement.Content.StartsWith(expectedAnnouncementPrefix));
        }
    }
}
