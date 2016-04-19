namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using LocalFileCondition;
    using Main.Services.Impl;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class DisableProductTest : NotAgentDirectLoginBaseTest
    {
        public void Run()
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();
            query.AnnouncementEnableStatus.Add(4523901, false);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }
    }
}
