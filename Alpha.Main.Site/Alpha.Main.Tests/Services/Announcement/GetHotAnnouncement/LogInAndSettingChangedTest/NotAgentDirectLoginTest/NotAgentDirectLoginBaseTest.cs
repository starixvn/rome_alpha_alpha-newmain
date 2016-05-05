namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest
{
    public class NotAgentDirectLoginBaseTest : LogInAndSettingChangedBaseTest
    {
        public NotAgentDirectLoginBaseTest()
        {
            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(HotAnnouncementsParams);
        }
    }
}
