namespace Alpha.Main.Site.Tests.Handlers
{
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Params;
    using Alpha.Main.ProfilePicture.Services;
    using AppResources;
    using Constants;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Main.Models;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Password;
    using SecurityCode.Services;
    using Services;
    using Site.Handlers;
    using Site.Requests.Password;
    using Site.ViewModels.Security;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using System;
    using Web.Helpers;

    [TestFixture]
    public class PasswordHandlerTests
    {
        private CompareLogic compareLogic;
        private PasswordHandler passwordHandler;
        private PasswordService passwordService;
        private IUserSession userSession;
        private SessionManager sessionManager;
        private Encryptor encryptor;
        private ProfilePictureServices customerProfileService;
        private SecurityCodeChecker securityCodeChecker;
        private IUser user;
        private NicknameChecker nicknameChecker;

        [SetUp]
        public void Setup()
        {
            compareLogic = new CompareLogic();
            passwordService = Substitute.For<PasswordService>();
            encryptor = Substitute.For<Encryptor>();
            customerProfileService = Substitute.For<ProfilePictureServices>();
            securityCodeChecker = Substitute.For<SecurityCodeChecker>();
            sessionManager = Substitute.For<SessionManager>();

            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);

            user = Substitute.For<IUser>();
            user.IsAdminCanUpdateSubAccount.Returns(true);
            nicknameChecker = Substitute.For<NicknameChecker>();

            SimpleServiceLocator
              .Instance
              .Register<Func<int>>(() => 1);

            passwordHandler = new PasswordHandler(
                passwordService,
                userSession,
                customerProfileService,
                sessionManager,
                encryptor,
                securityCodeChecker,
                user,
                nicknameChecker);
        }

        #region Handle Reset SubAccount Password Command

        [Test]
        public void HandleResetSubAccountPasswordCommand_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            ResetSubAccountPasswordCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleResetSubAccountPasswordCommand_LimitPermissionSubAccount_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsLimitedHostPermisionOnSubAccountFunction.Returns(true);
            var command = new ResetSubAccountPasswordCommand(userSession);

            // Act
            var result = passwordHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleResetSubAccountPasswordCommand_GetHashKeyFromSession_CallSessionManagerGet()
        {
            // Arrange
            var command = new ResetSubAccountPasswordCommand(userSession);

            // Act
            var result = passwordHandler.Handle(command);

            // Assert
            sessionManager.Received().Get<string>("Alpha.Main.PasswordHandler.PasswordHash");
        }

        [Test]
        public void HandleResetSubAccountPasswordCommand_MessageIsNotNullAndHasUpdatePermission_CallsResetSubAccountPassword()
        {
            // Arrange
            var command = new ResetSubAccountPasswordCommand(userSession);
            var expectedHashKey = "expectedHashKey";
            sessionManager.Get<string>(Arg.Any<string>()).Returns(expectedHashKey);

            // Act
            passwordHandler.Handle(command);

            // Assert
            passwordService.Received().ResetSubAccountPassword(
                Arg.Is<ResetSubAccountPasswordParam>(
                    param => param.ActorId == command.ActorId
                    && param.ActorName == command.ActorName
                    && param.CustId == command.CustId
                    && param.HashKey == expectedHashKey
                    && param.IP == command.IP
                    && param.LanguageKey == command.LanguageKey
                    && param.NewPassword == command.NewPassword
                    && param.SubAccountId == command.SubAccountId));
        }

        #endregion Handle Reset SubAccount Password Command

        #region Handle Change Password Query

        [Test]
        public void HandleChangePasswordQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangePasswordQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleChangePasswordQuery_MessageIsNotNull_ReturnPasswodViewModel()
        {
            // Arrange
            UrlHelperTestSetUp.SetUp();

            var hashKey = "HashKey";
            userSession.IsOfferProtectedMode.Returns(true);
            var message = new ChangePasswordQuery(userSession);

            encryptor.RandomString(Arg.Any<int>()).Returns(hashKey);
            securityCodeChecker.IsEnableSecurityCode().Returns(true);
            nicknameChecker.IsOfferForceLoginAndOverDueDate.Returns(false);

            var expectedViewModel = new PasswordViewModel
            {
                CustRoleId = message.CustRoleId,
                HashKey = hashKey,
                EnableSecurityCode = true,
                IsOfferProtectedMode = true,
                IsShowNicknameLink = true
            };

            // Act
            var actualViewModel = passwordHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        #endregion Handle Change Password Query

        #region Handle Reset MainAccount Password Command

        [Test]
        public void HandleResetMainAccountPasswordCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetMainAccountPasswordCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleResetMainAccountPasswordCommand_DoesNotHaveUpdatePermission_ReturnUnauthorizedResult()
        {
            // Arrange
            userSession.IsSubAcc.Returns(true);
            userSession.SubPageAccess.Returns(new PageAccess());

            var message = new ResetMainAccountPasswordCommand(userSession);

            // Act
            var result = passwordHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleResetMainAccountPasswordCommand_GetHashKeyFromSession_CallSessionManagerGet()
        {
            // Arrange
            var message = new ResetMainAccountPasswordCommand(userSession);
            message.CustRoleId = RoleId.Agent;

            // Act
            var result = passwordHandler.Handle(message);

            // Assert
            sessionManager.Received().Get<string>("Alpha.Main.PasswordHandler.PasswordHash");
        }

        [Test]
        public void HandleResetMainAccountPasswordCommand_MessageIsNotNullAndHasUpdatePermission_CallsResetMainAccountPassword()
        {
            // Arrange
            var message = new ResetMainAccountPasswordCommand(userSession);
            message.CustRoleId = RoleId.Agent;
            var expectedHashKey = "expectedHashKey";
            sessionManager.Get<string>(Arg.Any<string>()).Returns(expectedHashKey);

            // Act
            passwordHandler.Handle(message);

            // Assert
            passwordService.Received().ResetMainAccountPassword(
                Arg.Is<ResetMainAccountPasswordParam>(
                    param => param.ActorId == message.ActorId
                    && param.ActorName == message.ActorName
                    && param.CustId == message.CustId
                    && param.HashKey == expectedHashKey
                    && param.IP == message.IP                   
                    && param.LanguageKey == message.LanguageKey
                    && param.NewPassword == message.NewPassword
                    && param.SubAccountId == message.SubAccountId
                    && param.IsResetMemberPassword == false));
        }

        #endregion Handle Reset MainAccount Password Command

        #region Handle Change Password Command

        [Test]
        public void HandleChangePasswordCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangePasswordCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleChangePasswordCommand_DoesNotHaveUpdatePermission_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);

            var message = new ChangePasswordCommand(userSession);

            // Act
            var result = passwordHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HandleChangePasswordCommand_GetHashKeyFromSession_CallSessionManagerGet(bool isSubAccount)
        {
            // Arrange
            userSession.IsSubAcc.Returns(isSubAccount);
            var message = new ChangePasswordCommand(userSession);

            passwordService.ChangeSubAccountPassword(Arg.Any<ChangeSubAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            passwordService.ChangeMainAccountPassword(Arg.Any<ChangeMainAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            // Act
            var result = passwordHandler.Handle(message);

            // Assert
            sessionManager.Received().Get<string>("Alpha.Main.PasswordHandler.PasswordHash");
        }

        [Test]
        public void HandleChangePasswordCommand_IsSubAccount_CallsChangeSubAccountPassword()
        {
            // Arrange
            userSession.IsSubAcc.Returns(true);
            var message = new ChangePasswordCommand(userSession);

            var expectedHashKey = "expectedHashKey";
            sessionManager.Get<string>(Arg.Any<string>()).Returns(expectedHashKey);

            passwordService.ChangeSubAccountPassword(Arg.Any<ChangeSubAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            // Act
            passwordHandler.Handle(message);

            // Assert
            passwordService.Received().ChangeSubAccountPassword(
                Arg.Is<ChangeSubAccountPasswordParam>(
                    param => param.CustId == message.CustId
                    && param.HashKey == expectedHashKey
                    && param.IP == message.IP
                    && param.LanguageKey == message.LanguageKey
                    && param.NewPassword == message.NewPassword
                    && param.SubAccountId == message.SubAccountId
                    && param.CurrentPassword == message.CurrentPassword
                    && param.Nickname == message.NickName
                    && param.SubUsername == message.SubAccountUsername
                    && param.Username == message.Username));
        }

        [Test]
        public void HandleChangePasswordCommand_IsMainAccount_CallsChangeMainAccountPassword()
        {
            // Arrange
            userSession.IsSubAcc.Returns(false);
            var message = new ChangePasswordCommand(userSession);

            var expectedHashKey = "expectedHashKey";
            sessionManager.Get<string>(Arg.Any<string>()).Returns(expectedHashKey);

            passwordService.ChangeMainAccountPassword(Arg.Any<ChangeMainAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            // Act
            passwordHandler.Handle(message);

            // Assert
            passwordService.Received().ChangeMainAccountPassword(
                Arg.Is<ChangeMainAccountPasswordParam>(
                    param => param.ActorId == message.ActorId
                    && param.ActorName == message.Username
                    && param.CurrentPassword == message.CurrentPassword
                    && param.CustId == message.CustId
                    && param.HashKey == expectedHashKey
                    && param.IP == message.IP                 
                    && param.LanguageKey == message.LanguageKey
                    && param.CurrentPassword == message.CurrentPassword
                    && param.NewPassword == message.NewPassword
                    && param.NickName == message.NickName));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HandleChangePasswordCommand_ChangePasswordSuccessful_SetsUserSessionIsLoggedIsTrue(bool isSubAccount)
        {
            // Arrange
            userSession.IsSubAcc.Returns(isSubAccount);
            var message = new ChangePasswordCommand(userSession);

            passwordService.ChangeSubAccountPassword(Arg.Any<ChangeSubAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            passwordService.ChangeMainAccountPassword(Arg.Any<ChangeMainAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            // Act
            var result = passwordHandler.Handle(message);

            // Assert
            Assert.True(userSession.IsLogged);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HandleChangePasswordCommand_ChangePasswordSuccessful_SetsUserSessionIsPasswordExpiredIsFalse(bool isSubAccount)
        {
            // Arrange
            userSession.IsSubAcc.Returns(isSubAccount);
            var message = new ChangePasswordCommand(userSession);

            passwordService.ChangeSubAccountPassword(Arg.Any<ChangeSubAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            passwordService.ChangeMainAccountPassword(Arg.Any<ChangeMainAccountPasswordParam>())
                .Returns(new Result
                {
                    Code = 0
                });

            // Act
            var result = passwordHandler.Handle(message);

            // Assert
            Assert.False(userSession.IsPasswordExpired);
        }

        #endregion Handle Change Password Command

        #region Handle Force Change Password Query

        [Test]
        public void HandleForceChangePasswordQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ForceChangePasswordQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleForceChangePasswordQuery_MessageIsNotNull_CallsGetProfilePicture()
        {
            // Arrange
            var message = new ForceChangePasswordQuery(userSession);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(FakeDataHelper.Create<ActorProfilePictureModel>());

            // Act
            passwordHandler.Handle(message);

            // Assert
            customerProfileService.Received().GetActorProfilePicture(
                Arg.Is<int>(custId => custId == userSession.CustId));
        }

        [Test]
        public void HandleForceChangePasswordQuery_MainAccount_ReturnsForceChangePasswordViewModel()
        {
            // Arrange
            userSession.IsSubAcc.Returns(false);
            userSession.NickName.Returns(string.Empty);
            userSession.UserName.Returns("UserName");

            var message = new ForceChangePasswordQuery(userSession);
            encryptor.RandomString(Arg.Any<int>()).Returns("HashKey");

            var profilePicture = new ActorProfilePictureModel(null, 0, null);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(profilePicture);
            customerProfileService.UpdateActorProfilePicture(Arg.Any<UpdateActorProfilePictureParam>()).Returns(new Result() { Code = 0});
            
            var expectedViewModel = new ForceChangePasswordViewModel
            {
                HashKey = "HashKey",
                IsDefaultAvatar = true,
                Username = userSession.UserName,
                AlternateUsername = string.Empty,
                UsernameShower = "UserName",
                EmbedImage = string.Empty
            };

            var compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "EmbedImage", "UserLevel", "OldMainRootPath", "MainRootPath", "IsChangeMemberPassword" }
            });

            // Act
            var actualViewModel = passwordHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Test]
        public void HandleForceChangePasswordQuery_SubAccount_ReturnsForceChangePasswordViewModel()
        {
            // Arrange
            userSession.IsSubAcc.Returns(true);
            userSession.SubUserName.Returns("SubUserName");

            var message = new ForceChangePasswordQuery(userSession);
            encryptor.RandomString(Arg.Any<int>()).Returns("HashKey");

            var profilePicture = new ActorProfilePictureModel();
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(profilePicture);
            customerProfileService.UpdateActorProfilePicture(Arg.Any<UpdateActorProfilePictureParam>()).Returns(new Result() { Code = 0 });

            var expectedViewModel = new ForceChangePasswordViewModel
            {
                HashKey = "HashKey",
                IsDefaultAvatar = true,
                Username = userSession.UserName,
                AlternateUsername = "(SUB-ACC SubUserName)",
                UsernameShower = "SubUserName",
                EmbedImage = string.Empty
            };

            var compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "UserLevel", "OldMainRootPath", "MainRootPath", "IsChangeMemberPassword" }
            });

            // Act
            var actualViewModel = passwordHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        #endregion Handle Force Change Password Query

        #region Handle Reset MainAccount Password Query

        [Test]
        public void HandleResetMainAccountPasswordQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetMainAccountPasswordQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleResetMainAccountPasswordQuery_MessageIsNotNull_CallsHandleChangePasswordQuery()
        {
            // Arrange
            var message = new ResetMainAccountPasswordQuery
            {
                CustRoleId = RoleId.Agent
            };

            var hashKey = "HashKey";
            encryptor.RandomString(Arg.Any<int>()).Returns(hashKey);
            securityCodeChecker.IsEnableSecurityCode().Returns(true);

            var expectedViewModel = new PasswordViewModel
            {
                CustRoleId = message.CustRoleId,
                HashKey = hashKey,
                EnableSecurityCode = true,
                CustId = message.CustId,
                Title = "Reset Password",
                IsShowNicknameLink = true
            };

            // Act
            var actualViewModel = passwordHandler.Handle(message);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.CustRoleId, actualViewModel.CustRoleId).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.SubAccountId, actualViewModel.SubAccountId).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.HashKey, actualViewModel.HashKey).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.EnableSecurityCode, actualViewModel.EnableSecurityCode).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.Title, actualViewModel.Title).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.IsShowNicknameLink, actualViewModel.IsShowNicknameLink).AreEqual);

        }

        #endregion Handle Reset MainAccount Password Query

        #region Handle Reset SubAccount Password Query

        [Test]
        public void HandleResetSubAccountPasswordQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetSubAccountPasswordQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordHandler.Handle(message));
        }

        [Test]
        public void HandleResetSubAccountPasswordQuery_MessageIsNotNull_CallsHandleChangePasswordQuery()
        {
            // Arrange
            var message = new ResetSubAccountPasswordQuery
            {
                SubAccountId = 1,
                Username = "UserName"
            };

            var hashKey = "HashKey";
            encryptor.RandomString(Arg.Any<int>()).Returns(hashKey);
            securityCodeChecker.IsEnableSecurityCode().Returns(true);

            var expectedViewModel = new PasswordViewModel
            {
                CustRoleId = userSession.RoleId,
                SubAccountId = message.SubAccountId,
                HashKey = hashKey,
                EnableSecurityCode = true,
                Title = "Reset Password - UserName",
                IsShowNicknameLink = true
            };

            // Act
            var actualViewModel = passwordHandler.Handle(message);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.CustRoleId, actualViewModel.CustRoleId).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.SubAccountId, actualViewModel.SubAccountId).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.HashKey, actualViewModel.HashKey).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.EnableSecurityCode, actualViewModel.EnableSecurityCode).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.Title, actualViewModel.Title).AreEqual);
            Assert.IsTrue(compareLogic.Compare(expectedViewModel.IsShowNicknameLink, actualViewModel.IsShowNicknameLink).AreEqual);
        }

        #endregion Handle Reset SubAccount Password Query
    }
}