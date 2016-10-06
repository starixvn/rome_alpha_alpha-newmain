namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.AgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Main.Models.Announcements;

    public class SettingNotChangedTest : LogInAndSettingChangedBaseTest
    {
        public HotAnnouncement Run(bool hasAnnouncement, string expectedAnnouncementContent)
        {
            var query = HotAnnouncementsParams;

            var logInAndSettingChangedCondition = new AgentDirectLoginAndSettingChangedCondition(false, false);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(
                logInAndSettingChangedCondition.DynamicRepository,
                ValidAnnouncementTimeCondition.DateTimeWrapper);
            var localFileAnnouncementCondition = LocalFileConditionFactory.Create(hasAnnouncement);
            localFileAnnouncementCondition.SetUpCondition(query);

            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}