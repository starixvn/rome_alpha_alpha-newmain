namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services.Impl;
    using Criteria.Announcement;
    using Fanex.Data.Repository;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using Helps;
    using Main.Constants;
    using Main.Services;
    using Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Announcements;
    using System.Collections.Generic;

    [TestFixture]
    public class AnnouncementServiceImplTests
    {
        private const string EmptyAnnoucementPath = @"\TestData\Announcements\EmptyAnnouncement.xml";
        private const string AvailableAnnoucementPath = @"\TestData\Announcements\AvailableAnnouncement.xml";
        private const int ValidAdminId = 1;

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
            Assert.AreEqual(actualHotAnnouncement.Content, null);
        }

        [TestCase(true, true, "Your minimum position taking and betting limit value have been changed by the upline.")]
        [TestCase(true, false, "Your minimum position taking value has been changed by the upline.")]
        [TestCase(false, true, "Your betting limit value has been changed by the upline.")]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingChanged_SettingChangedAnnouncement(
            bool isPositionTakingChanged,
            bool isBettingLimitChanged,
            string expectedAnnouncementPrefix)
        {
            // Arrange
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var settingChanges = GetSettingChanges(isPositionTakingChanged, isBettingLimitChanged);
            dynamicRepository.Fetch<string>(Arg.Any<GetMessageTypeListCriteria>()).Returns(settingChanges);

            var announcementServiceImpl = new AnnouncementServiceImpl(dynamicRepository, null);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.RoleId = UserLevel.Agent.Value;
            query.AdminId = 0;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreNotEqual(actualHotAnnouncement, null);
            Assert.AreEqual(actualHotAnnouncement.Content.StartsWith(expectedAnnouncementPrefix), true);
        }

        [TestCase(true, ExpectAnnouncementContent)]
        [TestCase(false, null)]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingNotChange_AnnouncementFromFiles(
            bool hasAnnouncement,
            string expectedAnnouncementContent)
        {
            // Arrange
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            List<string> settingChanges = null;
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
            Assert.AreEqual(actualHotAnnouncement.Content, expectedAnnouncementContent);
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
            Assert.AreEqual(actualHotAnnouncement.Content, expectedAnnouncement);
        }

        [TestCase(EmptyAnnoucementPath, AvailableAnnoucementPath)]
        [TestCase(AvailableAnnoucementPath, EmptyAnnoucementPath)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndHasOnePrivateOrCommonAnnoucement_NotNullContent(
            string commonAnnouncementPath,
            string privateMessagePath)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService);

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
            Assert.AreNotEqual(actualHotAnnouncement.Content, null);
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
