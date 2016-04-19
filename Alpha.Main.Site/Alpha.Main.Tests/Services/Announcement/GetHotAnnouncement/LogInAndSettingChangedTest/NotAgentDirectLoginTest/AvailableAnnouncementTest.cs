namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using NUnit.Framework;

    public class AvailableAnnouncementTest : NotAgentDirectLoginBaseTest
    {
        public void Run(string availableAnnouncementPath)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        public void Run(
            string language,
            string availableAnnoucementPath,
            string expectedAnnouncementContent)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnoucementPath);
            localFileAnnouncementCondition.SetUpCondition(query);
            query.Language = language;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }
    }
}
