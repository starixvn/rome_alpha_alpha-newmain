namespace Alpha.Main.Tests.Services
{
    using System;
    using Alpha.Main.Services.Impl;
    using Announcement.GetHotAnnouncement.LocalFileCondition;
    using Announcement.GetHotAnnouncement.LogInAndSettingChangedTest;
    using Main.Constants;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Announcements;
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

        [Test]
        public void GetHotAnnouncement_NullParams_ThrowArgumentNullException()
        {
            // Arrange
            GetHotAnnouncementsParams nullParam = null;

            var announcementServiceImpl = new AnnouncementServiceImpl();

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => announcementServiceImpl.GetHotAnnouncement(nullParam)));
        }

        [Test]
        public void GetHotAnnouncement_NotPassSecurityCode_ReturnAnnouncementWithNullContent()
        {
            // Arrange
            //var securityCodeSession = Substitute.For<SecurityCodeSession>();
            //securityCodeSession.IsRequireSecurityCode.Returns(true);

            var announcementServiceImpl = new AnnouncementServiceImpl();

            var query = new GetHotAnnouncementsParams();

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
        public void GetHotAnnouncement_AgentDirectLoginAndSettingChanged_ReturnSettingChangedAnnouncement(
            bool isPositionTakingChanged,
            bool isBettingLimitChanged,
            bool isInsertPersonalMessage,
            string expectedAnnouncementPrefix)
        {
            // Arrange
            var test = new AgentDirectLoginTest.SettingChangedTest();

            // Action
            var actualHotAnnouncement = test.Run(
                isPositionTakingChanged,
                isBettingLimitChanged,
                isInsertPersonalMessage,
                expectedAnnouncementPrefix);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement);
            Assert.AreEqual(true, actualHotAnnouncement.Content.StartsWith(expectedAnnouncementPrefix));
        }

        [TestCase(true, ExpectedAnnouncementContent)]
        [TestCase(false, null)]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingNotChanged_ReturnAnnouncementFromFiles(
            bool hasAnnouncement,
            string expectedAnnouncementContent)
        {
            // Arrange
            var test = new AgentDirectLoginTest.SettingNotChangedTest();

            // Action
            var actualHotAnnouncement = test.Run(hasAnnouncement, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
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
        public void GetHotAnnouncement_NotAgentOrAdminLogInTest_ReturnAnnouncementFromFiles(
            byte roleId,
            bool isAdminLogIn,
            bool hasAnnouncement,
            string expectedAnnouncement)
        {
            // Arrange
            var test = new NotAgentOrAdminLogInTest();

            // Action
            var actualHotAnnouncement = test.Run(
                roleId,
                isAdminLogIn,
                hasAnnouncement,
                expectedAnnouncement);

            // Assert
            Assert.AreEqual(expectedAnnouncement, actualHotAnnouncement.Content);
        }

        [TestCase(LocalFileBaseCondition.EmptyAnnouncementPath, LocalFileBaseCondition.AvailableAnnoucementPath)]
        [TestCase(LocalFileBaseCondition.AvailableAnnoucementPath, LocalFileBaseCondition.EmptyAnnouncementPath)]
        [TestCase(LocalFileBaseCondition.AvailableAnnoucementPath, LocalFileBaseCondition.AvailableAnnoucementPath)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndHasPrivateOrCommonAnnouncement_ReturnNotNullContent(
            string commonAnnouncementPath,
            string privateMessagePath)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.HasPrivateOrCommonAnnouncementTest();

            // Action
            var actualHotAnnouncement = test.Run(commonAnnouncementPath, privateMessagePath);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotHasStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotReachStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_OverEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExpiredAnnouncement_ReturnNullContent(string expiredAnnouncementPath)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.ExpiredAnnouncementTest();

            // Action
            var actualHotAnnouncement = test.Run(expiredAnnouncementPath);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreAllSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnorePrivateSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", ExludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExcludeHotMessage_ReturnNullContent(string availableAnnouncementPath, string site)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.ExcludeHotMessageTest();

            // Action
            var actualHotAnnouncement = test.Run(availableAnnouncementPath, site);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", IncludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndIncludeHotMessage_ReturnNotNullContent(string availableAnnouncementPath, string site)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.IncludeHotMessageTest();

            // Action
            var actualHotAnnouncement = test.Run(availableAnnouncementPath, site);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_NotHasEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_ReturnNotNullContent(string availableAnnouncementPath)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.AvailableAnnouncementTest();

            // Action
            var actualHotAnnouncement = test.Run(availableAnnouncementPath);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_NotMatchLanguage_NotHasDefault.xml", null)]
        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_NotMatchLanguage_HasDefault.xml", ExpectedAnnouncementContent)]
        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_MatchLanguage.xml", ExpectedAnnouncementContent)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_ReturnCorrectContent(
            string language,
            string availableAnnoucementPath,
            string expectedAnnouncementContent)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.AvailableAnnouncementTest();

            // Action
            var actualHotAnnouncement = test.Run(language, availableAnnoucementPath, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_NotAgentDirectLoginAndDisableProduct_NullContent()
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.DisableProductTest();

            // Action
            var actualHotAnnouncement = test.Run();

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(ShowedAnnouncementId, null)]
        [TestCase(StillWaitAnnouncementId, ExpectedAnnouncementContent)]
        [TestCase("", ExpectedAnnouncementContent)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndShowedAnnouncement_ReturnCorrectContent(
            string showedAnnouncementId,
            string expectedAnnouncementContent)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.ShowedAnnouncementTest();

            // Action
            var actualHotAnnouncement = test.Run(showedAnnouncementId, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_DisableSecurityCodeAndValidAnnouncement_NotNullContent()
        {
            // Arrange
            var test = new DisableSecurityCodeAndValidAnnouncementTest();

            // Action
            var actualHotAnnouncement = test.Run();

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\OnlyForceUseSecurityCode.xml", null)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\HasAnnouncement.xml", ExpectedAnnouncementContent)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\EmptyAnnouncement.xml", null)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndNotForceUseSecurityCodeSite_ReturnCorrectContent(
            string announcementPath,
            string expectedAnnouncementContent)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.NotForceUseSecurityCodeSiteTest();

            // Action
            var actualHotAnnouncement = test.Run(announcementPath, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\OnlyForceUseSecurityCode.xml", ExpectedAnnouncementContent)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\HasAnnouncement.xml", ExpectedAnnouncementContent)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\EmptyAnnouncement.xml", null)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndForceUseSecurityCodeSite_ReturnCorrectContent(
            string announcementPath,
            string expectedAnnouncementContent)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.ForceUseSecurityCodeSiteTest();

            // Action
            var actualHotAnnouncement = test.Run(announcementPath, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }
    }
}
