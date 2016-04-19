namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using NUnit.Framework;

    public class IncludeHotMessageTest : NotAgentDirectLoginBaseTest
    {
        public void Run(string availableAnnouncementPath, string site)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            query.Site = site;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }
    }
}
