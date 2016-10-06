namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Main.Models.Announcements;

    public class ShowedAnnouncementTest : NotAgentDirectLoginBaseTest
    {
        public HotAnnouncement Run(string showedAnnouncementId, string expectedAnnouncementContent)
        {
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);
            query.MessageIds = showedAnnouncementId.ToString();

            var announcementServiceImpl = new AnnouncementServiceImpl(null, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}