namespace Alpha.Main.Site.Tests.Handlers
{
    using Constants;
    using Main.Models.Announcements;
    using Models.Requests.Announcement;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Announcements;
    using Services;
    using Sessions;
    using Site.Handlers;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AnnouncementHandlerTests
    {
        private AnnouncementService announcementService;
        private ISecurityCodeSession securityCodeSession;
        private AnnouncementHandler announcementHandler;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            announcementService = Substitute.For<AnnouncementService>();
            securityCodeSession = Substitute.For<ISecurityCodeSession>();
            announcementHandler = new AnnouncementHandler(announcementService, securityCodeSession);
            userSession = Substitute.For<IUserSession>();
        }

        [Test]
        public void HandleHotAnnouncementQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            HotAnnouncementQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => announcementHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleHotAnnouncementQuery_NotNullQuery_CallGetHotAnnouncement()
        {
            // Arrange
            var hotAnnouncement = new HotAnnouncement();
            announcementService.GetHotAnnouncement(Arg.Any<GetHotAnnouncementsParams>())
                .Returns(hotAnnouncement);

            userSession.RoleId.Returns(RoleId.ValidRoleId);
            var query = new HotAnnouncementQuery(new Dictionary<int, bool>(), userSession, string.Empty, string.Empty, false);

            // Action
            var viewModel = announcementHandler.Handle(query);

            // Assert
            announcementService.Received().GetHotAnnouncement(Arg.Any<GetHotAnnouncementsParams>());
        }

        [TestCase(0, 0, "6291552")]
        [TestCase(1176086, 0, "27066781")]
        [TestCase(0, 1, "6357089")]
        [TestCase(1176086, 1, "27132318")]
        public void HandleHotAnnouncementQuery_NotNullQuery_HotAnnouncementWithEncodedCookieByUserIdAndSubAccId(
            int userId,
            int subAccId,
            string expectedResult)
        {
            // Arrange
            var hotAnnouncement = new HotAnnouncement();
            announcementService.GetHotAnnouncement(Arg.Any<GetHotAnnouncementsParams>())
                .Returns(hotAnnouncement);

            userSession.RoleId.Returns(RoleId.ValidRoleId);
            userSession.UserId.Returns(userId);
            userSession.SubAccId.Returns(subAccId);

            var query = new HotAnnouncementQuery(new Dictionary<int, bool>(), userSession, string.Empty, string.Empty, false);

            // Action
            var viewModel = announcementHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedResult, viewModel.CookieId);
        }
    }
}
