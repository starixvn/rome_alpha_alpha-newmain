namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Services.Impl;
    using Announcement.GetHotAnnouncement.LocalFileCondition;
    using Announcement.GetHotAnnouncement.LogInAndSettingChangedTest;
    using Helps;
    using Main.Constants;
    using Main.Helpers;
    using NUnit.Framework;
    using Params.Announcements;
    using System;
    using System.Collections.Generic;
    using System.Xml;
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

            // Act & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => announcementServiceImpl.GetHotAnnouncement(nullParam)));
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

            // Act
            var actualHotAnnouncement = test.Run(
                isPositionTakingChanged,
                isBettingLimitChanged,
                isInsertPersonalMessage,
                expectedAnnouncementPrefix);

            // Assert
            Assert.IsNotNull(actualHotAnnouncement);
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

            // Act
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

            // Act
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

            // Act
            var actualHotAnnouncement = test.Run(commonAnnouncementPath, privateMessagePath);

            // Assert
            Assert.IsNotNull(actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotHasStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_NotReachStartDate.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Expired\Announcement_OverEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExpiredAnnouncement_ReturnNullContent(string expiredAnnouncementPath)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.ExpiredAnnouncementTest();

            // Act
            var actualHotAnnouncement = test.Run(expiredAnnouncementPath);

            // Assert
            Assert.IsNull(actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreAllSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnorePrivateSystem.xml", PublicSystem)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", ExludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndExcludeHotMessage_ReturnNullContent(string availableAnnouncementPath, string site)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.ExcludeHotMessageTest();

            // Act
            var actualHotAnnouncement = test.Run(availableAnnouncementPath, site);

            // Assert
            Assert.IsNull(actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Exclude\Announcement_IgnoreExcludedSite.xml", IncludedSite)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndIncludeHotMessage_ReturnNotNullContent(string availableAnnouncementPath, string site)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.IncludeHotMessageTest();

            // Act
            var actualHotAnnouncement = test.Run(availableAnnouncementPath, site);

            // Assert
            Assert.IsNotNull(actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml")]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\Announcement_NotHasEndDate.xml")]
        public void GetHotAnnouncement_NotAgentDirectLoginAndAvailableAnnouncement_ReturnNotNullContent(string availableAnnouncementPath)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.AvailableAnnouncementTest();

            // Act
            var actualHotAnnouncement = test.Run(availableAnnouncementPath);

            // Assert
            Assert.IsNotNull(actualHotAnnouncement.Content);
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

            // Act
            var actualHotAnnouncement = test.Run(language, availableAnnoucementPath, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_NotAgentDirectLoginAndDisableProduct_NullContent()
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.DisableProductTest();

            // Act
            var actualHotAnnouncement = test.Run();

            // Assert
            Assert.IsNull(actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_NotAgentDirectLoginAndEnableProduct_ReturnsAnnouncementContent()
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.EnableProductTest();

            // Act
            var actualHotAnnouncement = test.Run();

            // Assert
            Assert.AreEqual("Announcement", actualHotAnnouncement.Content);
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

            // Act
            var actualHotAnnouncement = test.Run(showedAnnouncementId, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void GetHotAnnouncement_DisableSecurityCodeAndValidAnnouncement_NotNullContent()
        {
            // Arrange
            var test = new DisableSecurityCodeAndValidAnnouncementTest();

            // Act
            var actualHotAnnouncement = test.Run();

            // Assert
            Assert.IsNotNull(actualHotAnnouncement.Content);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\Available\ForceUseSecurityCode\HasAnnouncement.xml", ExpectedAnnouncementContent)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\EmptyAnnouncement.xml", null)]
        public void GetHotAnnouncement_NotAgentDirectLoginAndNotForceUseSecurityCodeSite_ReturnCorrectContent(
            string announcementPath,
            string expectedAnnouncementContent)
        {
            // Arrange
            var test = new NotAgentDirectLoginTest.NotForceUseSecurityCodeSiteTest();

            // Act
            var actualHotAnnouncement = test.Run(announcementPath);

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

            // Act
            var actualHotAnnouncement = test.Run(announcementPath, expectedAnnouncementContent);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        [Test]
        public void IsShowForCustomerIds_NodeWithNoCustIdsAttribute_ReturnsTrue()
        {
            // Arrange
            XmlNode node = GetFirstNodeFromXmlDocument(@"\TestData\Announcements\HotAnnouncement\ByCustIds\NotHaveCustIdsAttr.xml");
            var AnnouncementServiceImplannouncementServiceImpl = new AnnouncementServiceImpl();

            // Act
            var actualIsShowForCustomerIds = AnnouncementServiceImpl.IsShowForCustomerIds(node, 0);

            // Assert
            Assert.True(actualIsShowForCustomerIds);
        }

        [Test]
        public void IsShowForCustomerIds_NodeWithEmptyCustIdsAttribute_ReturnsTrue()
        {
            // Arrange
            XmlNode node = GetFirstNodeFromXmlDocument(@"\TestData\Announcements\HotAnnouncement\ByCustIds\EmptyCustIdsAttr.xml");

            // Act
            var actualIsShowForCustomerIds = AnnouncementServiceImpl.IsShowForCustomerIds(node, 0);

            // Assert
            Assert.True(actualIsShowForCustomerIds);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 1, true)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 123, true)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 5, true)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 12, false)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 100, false)]
        public void IsShowForCustomerIds_NodeWithExistCusIdInCustIdsAttribute_ReturnsTrue(string xmlPath, int custId, bool expectedValue)
        {
            // Arrange
            XmlNode node = GetFirstNodeFromXmlDocument(xmlPath);          

            // Act
            var actualIsShowForCustomerIds = AnnouncementServiceImpl.IsShowForCustomerIds(node, custId);

            // Assert
            Assert.AreEqual(expectedValue, actualIsShowForCustomerIds);
        }

        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 1, ExpectedAnnouncementContent)]
        [TestCase(@"\TestData\Announcements\HotAnnouncement\ByCustIds\CustIdsAttr.xml", 100, null)]
        public void GetHotAnnouncement_NotAdminLoginAndShowForCustomerId_ReturnContentFromFile(string xmlPath, int custId, string expectedAnnouncementContent)
        {
            // Arrange
            var dateTimeWrapper = NSubstitute.Substitute.For<IDateTime>();
            dateTimeWrapper.Now = new DateTime(2017, 5, 1);

            var announcementServiceImpl = new AnnouncementServiceImpl(null, dateTimeWrapper);
            var getParams = new GetHotAnnouncementsParams
            {
                AdminId = 1,
                CustId = custId,
                PrivateMessagePath = FileHelper.GetFilePath(xmlPath),
                CommonAnnouncementPath = FileHelper.GetFilePath(xmlPath),
                MessageIds = string.Empty,
                AnnouncementEnableStatus = new Dictionary<int, bool>(),
                Language = "en-US"
            };

            // Act
            var actualHotAnnouncement = announcementServiceImpl.GetHotAnnouncement(getParams);

            // Assert
            Assert.AreEqual(expectedAnnouncementContent, actualHotAnnouncement.Content);
        }

        private static XmlNode GetFirstNodeFromXmlDocument(string xmlPath)
        {
            var xmlDocument = XMLDocumentHelper.LoadDocumentFromPath(
                FileHelper.GetFilePath(xmlPath));
            var node = xmlDocument.SelectNodes("FileDB/PopupAnnouncement")[0];
            return node;
        }
    }
}