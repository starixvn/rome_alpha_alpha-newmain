namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using NUnit.Framework;

    public class ShowedAnnouncementTest : NotAgentDirectLoginBaseTest
    {
        public void Run(string showedAnnouncementId, string expectedAnnouncementContent)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);
            query.MessageIds = showedAnnouncementId.ToString();

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }
    }
}
