namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using NUnit.Framework;

    public class NotAgentOrAdminLogInTest : LogInAndSettingChangedBaseTest
    {
        public void Run(
            byte roleId,
            bool isAdminLogIn,
            bool hasAnnouncement,
            string expectedAnnouncement)
        {
            // Arrange
            var query = HotAnnouncementsParams;

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition(roleId, isAdminLogIn);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService);

            var localFileAnnouncementCondition = LocalFileConditionFactory.Create(hasAnnouncement);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncement, actualHotAnnouncement.Content);
        }
    }
}
