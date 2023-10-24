namespace Alpha.Main.Site.Tests.ViewModels
{
    using System;
    using Alpha.Main.Configurations;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Helpers;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.ViewModels;

    [TestFixture]
    public class IndexViewModelTests
    {
        private IUserSession userSession;
        private BrowserHelper browserHelper;
        private IndexQuery query;
        private ActorProfilePictureModel profilePicture;
        private NicknameChecker nicknameChecker;

        [SetUp]
        public void SetUp()
        {
            AppSettings.Settings["UMAnnouncementPopup:ClientIntervalInMinutes"] = "60";
            userSession = Substitute.For<IUserSession>();
            userSession.SubPageAccess.Returns(new PageAccess());
            nicknameChecker = Substitute.For<NicknameChecker>();
            query = new IndexQuery(userSession, false, false, nicknameChecker);
            profilePicture = new ActorProfilePictureModel();
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            browserHelper = Substitute.For<BrowserHelper>();
        }

        [Test]
        public void InitialScaleViewPort_BrowserIsChromeInAndroidMobile_ReturnsCorrectValue()
        {
            // Arrange
            var device = new Device()
            {
                Id = "ID",
                Info = new DeviceInfo
                {
                    IP = "127.0.0.1",
                    BrowserName = "Chrome",
                    BrowserVersion = "BrowserVersion",
                    DeviceName = "DeviceName"
                },
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            browserHelper.GetDevice().Returns(device);
            browserHelper.GetUserAgent().Returns("android");

            // Act
            var viewModel = new IndexViewModel(query, null, null, browserHelper, profilePicture, false, null);

            // Assert
            Assert.AreEqual(0.5, viewModel.InitialScaleViewPort);
        }

        [Test]
        public void InitialScaleViewPort_BrowserIsNotChromeInAndroidMobile_ReturnsCorrectValue()
        {
            // Arrange
            var device = new Device()
            {
                Id = "ID",
                Info = new DeviceInfo
                {
                    IP = "127.0.0.1",
                    BrowserName = "firefox",
                    BrowserVersion = "BrowserVersion",
                    DeviceName = "DeviceName"
                },
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            browserHelper.GetDevice().Returns(device);
            browserHelper.GetUserAgent().Returns("ios");
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            // Act
            var viewModel = new IndexViewModel(query, null, null, browserHelper, profilePicture, false, null);

            // Assert
            Assert.AreEqual(0.7, viewModel.InitialScaleViewPort);
        }
    }
}