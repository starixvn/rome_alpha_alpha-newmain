namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LocalFileCondition;
    using NUnit.Framework;

    public class ForceUseSecurityCodeSiteTest : NotAgentDirectLoginBaseTest
    {
        public void Run(string announcementPath, string expectedAnnouncementContent)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(announcementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }
    }
}
