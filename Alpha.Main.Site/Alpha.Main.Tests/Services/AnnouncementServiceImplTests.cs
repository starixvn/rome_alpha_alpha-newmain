namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services.Impl;
    using Criteria.Announcement;
    using Entities.Announcements;
    using Fanex.Data.Repository;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using Helpers;
    using Helps;
    using Main.Constants;
    using Main.Services;
    using Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Announcements;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AnnouncementServiceImplTests
    {
        private const string EmptyAnnoucementPath = @"\TestData\Announcements\HotAnnouncement\EmptyAnnouncement.xml";
        private const string AvailableAnnoucementPath = @"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml";

        private const int ValidAdminId = 1;
        private const string PublicSystem = "publicSystem";
        private const string ExludedSite = "exludedSite";
        private const string IncludedSite = "includedSite";

        private const string ExpectedAnnouncementContent = "Announcement";

        private const string ShowedAnnouncementId = "4523901";
        private const string StillWaitAnnouncementId = "4523909";

        public const int PositionTakingChanged = 1;
        public const int BettingLimitChanged = 2;

        public const int OfferedGoldDeluxe = 3543501;
        public const int NotOfferedGoldDeluxe = 3543502;
        public const int OfferedSuperLive = 3577701;
        public const int OfferedColossusBets = 4523901;

        private DateTime validAnnouncementDay = new DateTime(2016, 4, 13);

        [Test]
        public void GetHotAnnouncement_RequireSecurityCode_AnnouncementWithNullContent()
        {
            // Arrange
            var announcementServiceImpl = new AnnouncementServiceImpl();

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(true);
            var query = new GetHotAnnouncementsParams(securityCodeSession, true);

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
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var settingChanges = GetSettingChanges(isPositionTakingChanged, isBettingLimitChanged);
            dynamicRepository.Fetch<string>(Arg.Any<GetMessageTypeListCriteria>()).Returns(settingChanges);

            IEnumerable<PersonalMessage> insertPersonalMessageResult = isInsertPersonalMessage ?
                   new List<PersonalMessage> { new PersonalMessage { Id = 0 } } :
                   null;
            dynamicRepository.Fetch<PersonalMessage>(Arg.Any<InsertPersonalMessageCriteria>()).Returns(insertPersonalMessageResult);

            var announcementServiceImpl = new AnnouncementServiceImpl(dynamicRepository, null);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.RoleId = UserLevel.Agent.Value;
            query.AdminId = 0;

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
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            List<string> settingChanges = GetSettingChanges(false, false);
            dynamicRepository.Fetch<string>(Arg.Any<GetMessageTypeListCriteria>()).Returns(settingChanges);

            var securityCodeService = Substitute.For<SecurityCodeService>();

            var announcementServiceImpl = new AnnouncementServiceImpl(dynamicRepository, securityCodeService);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.RoleId = UserLevel.Agent.Value;
            query.AdminId = 0;
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(hasAnnouncement ? AvailableAnnoucementPath : EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.Language = Language.Default;

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
            var securityCodeService = Substitute.For<SecurityCodeService>();

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.RoleId = roleId;
            query.AdminId = isAdminLogIn ? ValidAdminId : 0;
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(hasAnnouncement ? AvailableAnnoucementPath : EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.Language = Language.Default;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncement, actualHotAnnouncement.Content);
        }

        [TestCase(EmptyAnnoucementPath, AvailableAnnoucementPath)]
        [TestCase(AvailableAnnoucementPath, EmptyAnnoucementPath)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndHasOnePrivateOrCommonAnnoucement_NotNullContent(
            string commonAnnouncementPath,
            string privateMessagePath)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(commonAnnouncementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(privateMessagePath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;

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
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(expiredAnnouncementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreAllSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnorePrivateSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", ExludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExcludeHotMessage_NullContent(string availableAnnoucementPath, string site)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);
            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(availableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;
            query.Site = site;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", IncludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndIncludeHotMessage_NotNullContent(string availableAnnoucementPath, string site)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);
            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(availableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;
            query.Site = site;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_NotHasEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_NotNullContent(string availableAnnoucementPath)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(availableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;

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
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(availableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;
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
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();
            query.AnnouncementEnableStatus.Add(4523901, false);

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(AvailableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;
            query.Language = Language.Default;

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
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(AvailableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;
            query.MessageIds = showedAnnouncementId.ToString();

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_DisableSecurityCodeAndValidAnnouncement_NotNullContent()
        {
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeWrapper = Substitute.For<IDateTime>();
            dateTimeWrapper.Now.Returns(validAnnouncementDay);
            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeWrapper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, false);
            query.AnnouncementEnableStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(AvailableAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(null, actualHotAnnouncement.Content);
        }

        private List<string> GetSettingChanges(bool isPositionTakingChanged, bool isBettingLimitChanged)
        {
            var settingChanges = new List<string>();
            if (isPositionTakingChanged)
            {
                settingChanges.Add(PositionTakingChanged.ToString());
            }

            if (isBettingLimitChanged)
            {
                settingChanges.Add(BettingLimitChanged.ToString());
            }

            return settingChanges;
        }
    }
}
