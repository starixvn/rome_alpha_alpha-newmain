namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services.Impl;
    using Announcement.GetHotAnnouncement;
    using Main.Constants;
    using NUnit.Framework;
    using System.Collections.Generic;

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
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new AgentDirectLoginAndSettingChangedCondition(isPositionTakingChanged, isBettingLimitChanged, isInsertPersonalMessage);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(logInAndSettingChangedCondition.DynamicRepository);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement);
            Assert.AreEqual(true, actualHotAnnouncement.Content.StartsWith(expectedAnnouncementPrefix));
        }

        [TestCase(true, ExpectedAnnouncementContent)]
        [TestCase(false, null)]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingNotChange_AnnouncementFromFiles(
            bool hasAnnouncement,
            string expectedAnnouncementContent)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new AgentDirectLoginAndSettingChangedCondition(false, false);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(
                logInAndSettingChangedCondition.DynamicRepository,
                showedMessageCondition.SecurityCodeService);
            var localFileAnnouncementCondition = LocalFileConditionFactory.Create(hasAnnouncement);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

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
        public void GetHotAnnouncement_NotAgentDirectLogin_AnnouncementFromFiles(
            byte roleId,
            bool isAdminLogIn,
            bool hasAnnouncement,
            string expectedAnnouncement)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService);

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition(roleId, isAdminLogIn);
            logInAndSettingChangedCondition.SetUpCondition(query);

            var localFileAnnouncementCondition = LocalFileConditionFactory.Create(hasAnnouncement);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncement, actualHotAnnouncement.Content);
        }

        [TestCase(LocalFileCondition.EmptyAnnouncementPath, LocalFileCondition.AvailableAnnoucementPath)]
        [TestCase(LocalFileCondition.AvailableAnnoucementPath, LocalFileCondition.EmptyAnnouncementPath)]
        [TestCase(LocalFileCondition.AvailableAnnoucementPath, LocalFileCondition.AvailableAnnoucementPath)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndHasPrivateOrCommonAnnouncement_NotNullContent(
            string commonAnnouncementPath,
            string privateMessagePath)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(commonAnnouncementPath, privateMessagePath);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotHasStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotReachStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_OverEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExpiredAnnouncement_NullContent(string expiredAnnouncementPath)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(null, expiredAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreAllSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnorePrivateSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", ExludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExcludeHotMessage_NullContent(string availableAnnouncementPath, string site)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(null, availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            query.Site = site;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", IncludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndIncludeHotMessage_NotNullContent(string availableAnnouncementPath, string site)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            query.Site = site;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_NotHasEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_NotNullContent(string availableAnnouncementPath)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnouncementPath);
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_NotMatchLanguage_NotHasDefault.xml", null)]
        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_NotMatchLanguage_HasDefault.xml", ExpectedAnnouncementContent)]
        [TestCase(Language.Thai, @"\TestData\Announcements\HotAnnouncement\Language\Announcement_MatchLanguage.xml", ExpectedAnnouncementContent)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_AnnouncementContent(
            string language,
            string availableAnnoucementPath,
            string expectedAnnouncementContent)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasPrivateOrCommonAnnouncementCondition(availableAnnoucementPath);
            localFileAnnouncementCondition.SetUpCondition(query);
            query.Language = language;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_NotAgentDirectLoginAndDisableProduct_NullContent()
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();
            query.AnnouncementEnableStatus.Add(4523901, false);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(ShowedAnnouncementId, null)]
        [TestCase(StillWaitAnnouncementId, ExpectedAnnouncementContent)]
        [TestCase("", ExpectedAnnouncementContent)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndShowedAnnouncement_NullContent(
            string showedAnnouncementId,
            string expectedAnnouncementContent)
        {
            // Arrange
            var securityCodeCondition = new NotRequireSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);
            query.MessageIds = showedAnnouncementId.ToString();

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_DisableSecurityCodeAndValidAnnouncement_NotNullContent()
        {
            // Arrange
            var securityCodeCondition = new DisableSecurityCodeCondition();
            var query = securityCodeCondition.HotAnnouncementsParams();

            var logInAndSettingChangedCondition = new NotAgentDirectLoginCondition();
            logInAndSettingChangedCondition.SetUpCondition(query);

            var showedMessageCondition = new ShowedMessageCondition(false);
            showedMessageCondition.SetUpCondition(query);

            var announcementTimeCondition = new ValidAnnouncementTimeCondition();
            var announcementServiceImpl = new AnnouncementServiceImpl(null, showedMessageCondition.SecurityCodeService, announcementTimeCondition.DateTimeWrapper);

            var localFileAnnouncementCondition = new HasAnnouncementCondition();
            localFileAnnouncementCondition.SetUpCondition(query);

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }
    }
}
