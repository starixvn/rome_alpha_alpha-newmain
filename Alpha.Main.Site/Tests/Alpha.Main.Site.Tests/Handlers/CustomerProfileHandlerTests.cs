#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.Main.Services;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Helpers;
    using AlphaPlus.User.Models;
    using Constants;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Models.Requests.CustomerProfile;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.ViewModels.CustomerProfile;

    [TestFixture]
    public class CustomerProfileHandlerTests
    {
        private ProfilePictureServices customerProfileService;
        private CustomerProfileHandler customerProfileHandler;
        private IUserSession userSession;
        private NicknameChecker nicknameChecker;

        [SetUp]
        public void SetUp()
        {
            customerProfileService = Substitute.For<ProfilePictureServices>();
            nicknameChecker = Substitute.For<NicknameChecker>();
            SimpleServiceLocator
             .Instance
             .Register<Func<int>>(() => 1);
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            customerProfileHandler = new CustomerProfileHandler(customerProfileService, userSession, nicknameChecker);
        }

        [Test]
        public void HandleBriefProfileQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            BriefProfileQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => customerProfileHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleBriefProfileQuery_NotNullQuery_CallGetProfilePictureService()
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            customerProfileService.Received().GetActorProfilePicture(Arg.Is<int>(custId => custId == query.CustId));
        }

        [TestCase(CustId.InvalidId, CustId.InvalidId)]
        [TestCase(CustId.ValidId, CustId.ValidId)]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnCorrectCustId(
            int custId,
            int expectedCustId)
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            userSession.CustId.Returns(custId);

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedCustId, viewModel.CustId);
        }

        [TestCase(Username.Base64Name, "Base64Name")]
        [TestCase(Username.ContainHtmlCode, "!@#$%^&amp;*()-+&lt;&gt;? ~.")]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnCorrectCustId(
            string username,
            string expectedUsername)
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            userSession.UserName.Returns(username);

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedUsername, viewModel.Username);
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnIsNotMainAccountMatchWithAccountType(
            bool isSubAccount,
            bool isInternal,
            bool expectedIsNotMainAccount)
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            userSession.IsSubAcc.Returns(isSubAccount);
            userSession.IsInternal.Returns(isInternal);

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedIsNotMainAccount, viewModel.IsInternal);
        }

        [TestCase(true, false, "")]
        [TestCase(true, true, "Admin ")]
        [TestCase(false, true, "Admin ")]
        [TestCase(false, false, "")]
        public void HandleBriefProfileQuery_QueryMatchProfileWithEmptyName_ReturnNickNameMatchWithAccountType(
            bool isSubAccount,
            bool isInternal,
            string expectedNickName)
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            userSession.IsSubAcc.Returns(isSubAccount);
            userSession.IsInternal.Returns(isInternal);
            userSession.SubUserName.Returns(string.Empty);
            userSession.AdminName.Returns(string.Empty);
            userSession.NickName.Returns(string.Empty);

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedNickName, viewModel.NickName);
        }

        [TestCase(CustId.InvalidId, "CCA80D2C89CCA8EC9280F5088DB181")]
        [TestCase(CustId.ValidId, "40C8226B76C76797A38A6AC830708A")]
        public void HandleBriefProfileQuery_QueryMatchProfileWithEmptyName_ReturnToolTipCookieMatchWithCustId(
            int custId,
            string expectedCookieToolTipName)
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            userSession.CustId.Returns(custId);

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedCookieToolTipName, viewModel.IntroduceNickNameTooltipCookieName);
        }

        [Test]
        public void HandleBriefProfileQuery_UserIsEnabledOtp_ReturnEnabledOtp()
        {
            // Arrange
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            var query = new BriefProfileQuery(userSession);

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.IsTrue(viewModel.IsEnableOtp);
        }

        [Test]
        public void HandleBriefProfileQuery_QueryIsNotNull_ReturnBriefProfileViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            UrlHelperTestSetUp.SetUp();

            userSession.CustId.Returns(CustId.ValidId);
            userSession.UserName.Returns(Username.Base64Name);
            userSession.NickName.Returns("NickName");

            customerProfileService.GetActorProfilePicture(Arg.Any<int>())
                .Returns(new ActorProfilePictureModel());

            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            var query = new BriefProfileQuery(userSession);
            var expectedViewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = true,
                EmbedImage = string.Empty,
                ProfileImageId = 0,
                CustId = query.CustId,
                Username = "Base64Name",
                IsInternal = false,
                NickName = "NickName",
                IntroduceNickNameTooltipCookieName = "40C8226B76C76797A38A6AC830708A",
                IsEnableOtp = true,
                SubAccountTooltip = string.Empty,
                IsHideNicknameLink = false,
                IsShowProfilePictureTooltip = true
            };

            // Act
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel, viewModel).AreEqual);
        }
    }
}