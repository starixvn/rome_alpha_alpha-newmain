namespace Alpha.Main.Site.Tests.ViewModels
{
    using System;
    using Alpha.Main.Models.CustomerProfiles;
    using Alpha.SecurityCode.Web;
    using Main.Models.Otp;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Helpers;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class IndexViewModelTests
    {
        private IUserSession userSession;
        private BrowserHelper browserHelper;
        private IndexQuery query;
        private ProfilePicture profilePicture;

        [SetUp]
        public void SetUp()
        {
          
            userSession = Substitute.For<IUserSession>();
            userSession.SubPageAccess.Returns(new PageAccess());
            query = new IndexQuery(userSession, false, false);
            profilePicture = new ProfilePicture(12, true);
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
            var viewModel = new IndexViewModel(query, null, null, browserHelper, profilePicture);

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
            var viewModel = new IndexViewModel(query, null, null, browserHelper, profilePicture);

            // Assert
            Assert.AreEqual(0.7, viewModel.InitialScaleViewPort);
        }
    }
}
