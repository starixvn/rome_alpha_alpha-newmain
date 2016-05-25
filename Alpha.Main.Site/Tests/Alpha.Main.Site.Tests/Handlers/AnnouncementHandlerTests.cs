namespace Alpha.Main.Site.Tests.Handlers
{
    using Constants;
    using Fanex.Razorbill.Security.Sessions;
    using Main.Models.Announcements;
    using Models.Requests.Announcement;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Announcements;
    using Services;
    using Site.Handlers;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AnnouncementHandlerTests
    {
        [Test]
        public void HandleHotAnnouncementQuery_NullQuery_ThrowNullReferenceException()
        {
            // Arrange
            var announcementService = Substitute.For<AnnouncementService>();
            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            var announcementHandler = new AnnouncementHandler(announcementService, securityCodeSession);

            // Action & Assert
            Assert.Throws<NullReferenceException>(new TestDelegate(() => announcementHandler.Handle(null)));
        }

        [Test]
        public void HandleHotAnnouncementQuery_NotNullQuery_CallGetHotAnnouncement()
        {
            // Arrange
            var hotAnnouncement = new HotAnnouncement();
            var announcementService = Substitute.For<AnnouncementService>();
            announcementService.GetHotAnnouncement(Arg.Any<GetHotAnnouncementsParams>())
                .Returns(hotAnnouncement);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            var announcementHandler = new AnnouncementHandler(announcementService, securityCodeSession);

            var userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.ValidRoleId);
            var query = Substitute.For<HotAnnouncementQuery>(new Dictionary<int, bool>(), userSession, string.Empty, string.Empty, false);

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
            var announcementService = Substitute.For<AnnouncementService>();
            announcementService.GetHotAnnouncement(Arg.Any<GetHotAnnouncementsParams>())
                .Returns(hotAnnouncement);

            var securityCodeSession = Substitute.For<ISecurityCodeSession>();
            var announcementHandler = new AnnouncementHandler(announcementService, securityCodeSession);

            var userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.ValidRoleId);
            userSession.UserId.Returns(userId);
            userSession.SubAccId.Returns(subAccId);

            var query = Substitute.For<HotAnnouncementQuery>(new Dictionary<int, bool>(), userSession, string.Empty, string.Empty, false);

            // Action
            var viewModel = announcementHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedResult, viewModel.CookieId);
        }
    }
}
