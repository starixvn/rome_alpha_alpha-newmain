namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    public class NotAgentDirectLoginBaseTest : LogInAndSettingChangedBaseTest
    {
        public NotAgentDirectLoginBaseTest()
        {
            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(HotAnnouncementsParams);

            ValidAnnouncementTimeCondition = new ValidAnnouncementTimeCondition();
        }

        public ValidAnnouncementTimeCondition ValidAnnouncementTimeCondition { get; set; }
    }
}
