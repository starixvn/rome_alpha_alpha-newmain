namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Fanex.Razorbill.Security.Sessions;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Announcements;

    [TestFixture]
    public class NotPassSecurityCodeTest
    {
        [Test]
        public void GetHotAnnouncement_NotPassSecurityCode_AnnouncementWithNullContent()
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
    }
}
