namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services.Impl;
    using Announcement.GetHotAnnouncement.LocalFileCondition;
    using Announcement.GetHotAnnouncement.LogInAndSettingChangedTest;
    using Announcement.GetHotAnnouncement.SecurityCodeCondition;
    using Main.Constants;
    using NUnit.Framework;
    using AgentDirectLoginTest = Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.AgentDirectLoginTest;
    using NotAgentDirectLoginTest = Announcement.GetHotAnnouncement.LogInAndSettingChangedTest.NotAgentDirectLoginTest;

    [TestFixture]
    public class AnnouncementServiceImplTests
    {
        private const string PublicSystem = "publicSystem";
        private const string ExludedSite = "exludedSite";
        private const string IncludedSite = "includedSite";

        private const string ExpectedAnnouncementContent = "Announcement";

        private const string ShowedAnnouncementId = "4523901";
        private const string StillWaitAnnouncementId = "4523909";

        public const string ForceUseSecurityCode = "20140512";

        [Test]
        public void GetHotAnnouncement_NotPassSecurityCode_AnnouncementWithNullContent()
        {
            // Arrange
            var announcementServiceImpl = new AnnouncementServiceImpl();

            var securityCodeCondition = new NotPassSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(true, true, true, "Your minimum position taking and betting limit value have been changed by the upline.")]
        [TestCase(true, false, true, "Your minimum position taking value has been changed by the upline.")]
        [TestCase(false, true, true, "Your betting limit value has been changed by the upline.")]
        [TestCase(true, true, false, "Your minimum position taking and betting limit value have been changed by the upline.")]
        [TestCase(true, false, false, "Your minimum position taking value has been changed by the upline.")]
        [TestCase(false, true, false, "Your betting limit value has been changed by the upline.")]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingChanged_SettingChangedAnnouncement(
            bool isPositionTakingChanged,
            bool isBettingLimitChanged,
            bool isInsertPersonalMessage,
            string expectedAnnouncementPrefix)
        {
            var test = new AgentDirectLoginTest.SettingChangedTest();
            test.Run(
                isPositionTakingChanged,
                isBettingLimitChanged,
                isInsertPersonalMessage,
                expectedAnnouncementPrefix);
        }

        [TestCase(true, ExpectedAnnouncementContent)]
        [TestCase(false, null)]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingNotChanged_AnnouncementFromFiles(
            bool hasAnnouncement,
            string expectedAnnouncementContent)
        {
            var test = new AgentDirectLoginTest.SettingNotChangedTest();
            test.Run(hasAnnouncement, expectedAnnouncementContent);
        }

        [TestCase(RoleId.Agent, true, true, ExpectedAnnouncementContent)]
        [TestCase(RoleId.Master, true, true, ExpectedAnnouncementContent)]
        [TestCase(RoleId.Super, true, true, ExpectedAnnouncementContent)]
        [TestCase(RoleId.Master, false, true, ExpectedAnnouncementContent)]
        [TestCase(RoleId.Super, false, true, ExpectedAnnouncementContent)]
        [TestCase(RoleId.Agent, true, false, null)]
        [TestCase(RoleId.Master, true, false, null)]
        [TestCase(RoleId.Super, true, false, null)]
        [TestCase(RoleId.Master, false, false, null)]
        [TestCase(RoleId.Super, false, false, null)]
        public void GetHotAnnouncement_NotAgentorAdminLogInTest_AnnouncementFromFiles(
            byte roleId,
            bool isAdminLogIn,
            bool hasAnnouncement,
            string expectedAnnouncement)
        {
            var test = new NotAgentOrAdminLogInTest();
            test.Run(
                roleId,
                isAdminLogIn,
                hasAnnouncement,
                expectedAnnouncement);
        }

        [TestCase(LocalFileBaseCondition.EmptyAnnouncementPath, LocalFileBaseCondition.AvailableAnnoucementPath)]
        [TestCase(LocalFileBaseCondition.AvailableAnnoucementPath, LocalFileBaseCondition.EmptyAnnouncementPath)]
        [TestCase(LocalFileBaseCondition.AvailableAnnoucementPath, LocalFileBaseCondition.AvailableAnnoucementPath)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndHasPrivateOrCommonAnnouncement_NotNullContent(
            string commonAnnouncementPath,
            string privateMessagePath)
        {
            var test = new NotAgentDirectLoginTest.HasPrivateOrCommonAnnouncementTest();
            test.Run(commonAnnouncementPath, privateMessagePath);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotHasStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotReachStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_OverEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExpiredAnnouncement_NullContent(string expiredAnnouncementPath)
        {
            var test = new NotAgentDirectLoginTest.ExpiredAnnouncementTest();
            test.Run(expiredAnnouncementPath);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreAllSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnorePrivateSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", ExludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExcludeHotMessage_NullContent(string availableAnnouncementPath, string site)
        {
            var test = new NotAgentDirectLoginTest.ExcludeHotMessageTest();
            test.Run(availableAnnouncementPath, site);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", IncludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndIncludeHotMessage_NotNullContent(string availableAnnouncementPath, string site)
        {
            var test = new NotAgentDirectLoginTest.IncludeHotMessageTest();
            test.Run(availableAnnouncementPath, site);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_NotHasEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_NotNullContent(string availableAnnouncementPath)
        {
            var test = new NotAgentDirectLoginTest.AvailableAnnouncementTest();
            test.Run(availableAnnouncementPath);
        }

        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_NotMatchLanguage_NotHasDefault.xml", null)]
        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_NotMatchLanguage_HasDefault.xml", ExpectedAnnouncementContent)]
        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_MatchLanguage.xml", ExpectedAnnouncementContent)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_AnnouncementContent(
            string language,
            string availableAnnoucementPath,
            string expectedAnnouncementContent)
        {
            var test = new NotAgentDirectLoginTest.AvailableAnnouncementTest();
            test.Run(language, availableAnnoucementPath, expectedAnnouncementContent);
        }

        [Test]
        public void GetHotAnnouncement_NotAgentDirectLoginAndDisableProduct_NullContent()
        {
            var test = new NotAgentDirectLoginTest.DisableProductTest();
            test.Run();
        }

        [TestCase(ShowedAnnouncementId, null)]
        [TestCase(StillWaitAnnouncementId, ExpectedAnnouncementContent)]
        [TestCase("", ExpectedAnnouncementContent)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndShowedAnnouncement_NullContent(
            string showedAnnouncementId,
            string expectedAnnouncementContent)
        {
            var test = new NotAgentDirectLoginTest.ShowedAnnouncementTest();
            test.Run(showedAnnouncementId, expectedAnnouncementContent);
        }

        [Test]
        public void GetHotAnnouncement_DisableSecurityCodeAndValidAnnouncement_NotNullContent()
        {
            var test = new NotAgentDirectLoginTest.DisableSecurityCodeAndValidAnnouncementTest();
            test.Run();
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\HasAnnouncement.xml", ExpectedAnnouncementContent)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\NotHasAnnouncement.xml", null)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndIsForceUseSecurityCodeSite_NotNullContent(
            string announcementPath,
            string expectedAnnouncementContent)
        {
            var test = new NotAgentDirectLoginTest.ForceUseSecurityCodeSiteTest();
            test.Run(announcementPath, expectedAnnouncementContent);
        }
    }
}
