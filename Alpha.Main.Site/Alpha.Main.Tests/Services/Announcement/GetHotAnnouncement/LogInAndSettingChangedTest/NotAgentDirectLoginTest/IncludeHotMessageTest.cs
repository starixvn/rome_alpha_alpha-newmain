namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    using Alpha.Main.Services.Impl;
    using LocalFileCondition;
    using Models.Announcements;

    public class IncludeHotMessageTest : NotAgentDirectLoginBaseTest
    {
        public HotAnnouncement Run(string availableAnnouncementPath, string site)
        {
            var query = HotAnnouncementsParams;

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, ValidAnnouncementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            query.Site = site;

            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            return actualHotAnnouncement;
        }
    }
}
