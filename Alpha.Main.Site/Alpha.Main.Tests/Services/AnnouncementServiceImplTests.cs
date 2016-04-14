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
        private const string EmptyAnnoucementPath = @"\TestData\Announcements\EmptyAnnouncement.xml";
        private const string AvailableAnnoucementPath = @"\TestData\Announcements\AvailableAnnouncement.xml";
        private const string ExpiredAnnoucementPath = @"\TestData\Announcements\ExpiredAnnouncement.xml";

        private const int ValidAdminId = 1;
        private const string PublicSystem = "publicSystem";
        private const string ExludedSite = "exludedSite";

        private const string ExpectAnnouncementContent = "Announcement";

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

        [TestCase(true, ExpectAnnouncementContent)]
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
            query.ProductsStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(hasAnnouncement ? AvailableAnnoucementPath : EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.Language = Language.Default;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [TestCase(RoleId.Agent, true, true, ExpectAnnouncementContent)]
        [TestCase(RoleId.Master, true, true, ExpectAnnouncementContent)]
        [TestCase(RoleId.Super, true, true, ExpectAnnouncementContent)]
        [TestCase(RoleId.Master, false, true, ExpectAnnouncementContent)]
        [TestCase(RoleId.Super, false, true, ExpectAnnouncementContent)]
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
            query.ProductsStatus = new Dictionary<int, bool>();

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
            var dateTimeHelper = Substitute.For<DateTimeHelper>();

            var validAnnouncementDay = new DateTime(2016, 4, 13);
            dateTimeHelper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeHelper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.ProductsStatus = new Dictionary<int, bool>();

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

        [Test]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExpiredAnnouncement_NullContent()
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeHelper = Substitute.For<DateTimeHelper>();

            var validAnnouncementDay = new DateTime(2016, 4, 13);
            dateTimeHelper.Now.Returns(validAnnouncementDay);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeHelper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.ProductsStatus = new Dictionary<int, bool>();

            query.CommonAnnouncementPath = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(ExpiredAnnoucementPath);
            query.Language = Language.Default;
            query.RoleId = RoleId.ValidRoleId;
            query.AdminId = ValidAdminId;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(null, actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\AvailableAnnouncement_IgnoreAllSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\AvailableAnnouncement_IgnorePrivateSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\AvailableAnnouncement_IgnoreExcludedSite.xml", ExludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExcludeHotMessage_NullContent(string availableAnnoucementPath, string site)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeHelper = Substitute.For<DateTimeHelper>();

            var validAnnouncementDay = new DateTime(2016, 4, 13);
            dateTimeHelper.Now.Returns(validAnnouncementDay);
            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeHelper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.ProductsStatus = new Dictionary<int, bool>();

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

        [Test]
        public void GetHotAnnouncement_DisableSecurityCodeAndValidAnnouncement_AnnouncementWithNullContent()
        {
            var securityCodeService = Substitute.For<SecurityCodeService>();
            var dateTimeHelper = Substitute.For<DateTimeHelper>();

            var validAnnouncementDay = new DateTime(2016, 4, 13);
            dateTimeHelper.Now.Returns(validAnnouncementDay);
            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService, dateTimeHelper);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, false);
            query.ProductsStatus = new Dictionary<int, bool>();

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
                settingChanges.Add(SettingChangeType.PositionTakingChanged.ToString());
            }

            if (isBettingLimitChanged)
            {
                settingChanges.Add(SettingChangeType.BettingLimitChanged.ToString());
            }

            return settingChanges;
        }
    }
}
