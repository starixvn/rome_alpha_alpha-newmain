namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services.Impl;
    using Constants;
    using Criteria.Announcement;
    using Fanex.Data.Repository;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using Helps;
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

        [Test]
        public void GetHotAnnouncement_RequireSecurityCode_AnnoucementWithNullContent()
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
        public void GetHotAnnouncement_AgentDirectLoginAndSettingChanged_SettingChangedAnnoucement(
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

        [Test]
        public void GetHotAnnouncement_AgentDirectLoginAndSettingNotChangeAndEmptyAnnouncement_AnnoucementWithNullContent()
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

            var emptyAnnoucement = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.CommonAnnouncementPath = emptyAnnoucement;
            query.PrivateMessagePath = emptyAnnoucement;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(actualHotAnnouncement.Content, null);
        }

        [TestCase(RoleId.Agent)]
        [TestCase(RoleId.Super)]
        [TestCase(RoleId.Master)]
        public void GetHotAnnouncement_AdminLoginAndEmptyAnnouncement_AnnoucementWithNullContent(byte roleId)
        {
            // Arrange
            var securityCodeService = Substitute.For<SecurityCodeService>();

            var announcementServiceImpl = new AnnouncementServiceImpl(null, securityCodeService);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            securityCodeSession.IsRequireSecurityCode.Returns(false);
            securityCodeSession.SecurityCodeResult.Returns((int)SecurityCodeStatus.NONE);

            var query = new GetHotAnnouncementsParams(securityCodeSession, true);
            query.RoleId = roleId;
            var anyAdminId = 1;
            query.AdminId = anyAdminId;
            query.ProductsStatus = new Dictionary<int, bool>();

            var emptyAnnoucement = FileHelper.GetFilePath(EmptyAnnoucementPath);
            query.CommonAnnouncementPath = emptyAnnoucement;
            query.PrivateMessagePath = emptyAnnoucement;

            // Action
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(query);

            // Assert
            Assert.AreEqual(actualHotAnnouncement.Content, null);
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
