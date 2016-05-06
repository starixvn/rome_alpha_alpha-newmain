namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Main.Models.Announcements;

    public class AvailableAnnouncementTest : NotAgentDirectLoginBaseTest
    {
        public HotAnnouncement Run(string availableAnnouncementPath)
        {
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }

        public HotAnnouncement Run(
            string language,
            string availableAnnoucementPath,
            string expectedAnnouncementContent)
        {
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnoucementPath);
            localFileAnnouncementCondition.SetUpCondition(query);
            query.Language = language;

            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}