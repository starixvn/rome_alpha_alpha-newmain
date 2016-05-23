namespace Alpha.Main.Site.Tests.Handlers
{
    using Constants;
    using Main.Models.CustomerProfiles;
    using Models.Requests.CustomerProfile;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Sunplus.Agent.Model.Entities;
    using System;

    [TestFixture]
    public class CustomerProfileHandlerTests
    {
        [Test]
        public void HandleBriefProfileQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            BriefProfileQuery query = null;

            var customerProfileService = Substitute.For<CustomerProfileService>();
            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action & Assert
            Assert.Throws<NullReferenceException>(new TestDelegate(() => customerProfileHandler.Handle(query)));
        }

        [Test]
        public void HandleBriefProfileQuery_NotNullQuery_CallGetProfilePictureService()
        {
            // Arrange
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture());

            var userSession = Substitute.For<IUserSession>();
            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            customerProfileService.Received().GetProfilePicture(Arg.Is<int>(custId => custId == query.CustId));
        }

        [TestCase(null, "")]
        [TestCase(new byte[0], "data:image/jpeg;base64,")]
        [TestCase(new byte[2] { (byte)1, (byte)2 }, "data:image/jpeg;base64,AQI=")]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnEmbedImage(
            byte[] binaryAvatar,
            string expectedEmbedImage)
        {
            // Arrange
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture
            {
                BinaryAvatar = binaryAvatar
            });

            var userSession = Substitute.For<IUserSession>();
            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedEmbedImage, viewModel.EmbedImage);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnCorrectIsDefaultAvatar(
            bool isDefaultAvatar,
            bool expectedIsDefaultAvatar)
        {
            // Arrange
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture
            {
                IsDefaultAvatar = isDefaultAvatar
            });

            var userSession = Substitute.For<IUserSession>();
            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedIsDefaultAvatar, viewModel.IsDefaultAvatar);
        }

        [TestCase(CustId.InvalidId, CustId.InvalidId)]
        [TestCase(CustId.ValidId, CustId.ValidId)]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnCorrectCustId(
            int custId,
            int expectedCustId)
        {
            // Arrange
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture());

            var userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(custId);

            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
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
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture());

            var userSession = Substitute.For<IUserSession>();
            userSession.UserName.Returns(username);

            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedUsername, viewModel.Username);
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public void HandleBriefProfileQuery_QueryMatchProfile_ReturnIsNotMainAccountMatchWithAccountType(
            bool isSubAccount,
            bool isInternal,
            bool expectedIsNotMainAccount)
        {
            // Arrange
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture());

            var userSession = Substitute.For<IUserSession>();
            userSession.IsSubAcc.Returns(isSubAccount);
            userSession.IsInternal.Returns(isInternal);

            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedIsNotMainAccount, viewModel.LoginUserIsNotMainAccount);
        }

        [TestCase(true, false, "Sub-Acc ")]
        [TestCase(true, true, "Sub-Acc ")]
        [TestCase(false, true, "Admin ")]
        [TestCase(false, false, "")]
        public void HandleBriefProfileQuery_QueryMatchProfileWithEmptyName_ReturnNickNameMatchWithAccountType(
            bool isSubAccount,
            bool isInternal,
            string expectedNickName)
        {
            // Arrange
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture());

            var userSession = Substitute.For<IUserSession>();
            userSession.IsSubAcc.Returns(isSubAccount);
            userSession.IsInternal.Returns(isInternal);
            userSession.SubUserName.Returns(string.Empty);
            userSession.AdminName.Returns(string.Empty);
            userSession.NickName.Returns(string.Empty);

            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
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
            var customerProfileService = Substitute.For<CustomerProfileService>();
            customerProfileService.GetProfilePicture(Arg.Any<int>()).Returns(new ProfilePicture());

            var userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(custId);

            var query = new BriefProfileQuery(userSession);

            var customerProfileHandler = new CustomerProfileHandler(customerProfileService);

            // Action
            var viewModel = customerProfileHandler.Handle(query);

            // Assert
            Assert.AreEqual(expectedCookieToolTipName, viewModel.IntroduceNickNameTooltipCookieName);
        }
    }
}
