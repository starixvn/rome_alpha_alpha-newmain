namespace Alpha.Main.Site.Tests.Handlers
{
    using Alpha.ProfilePicture.Models;
    using Alpha.ProfilePicture.Services;
    using AppResources;
    using Helpers;
    using KellermanSoftware.CompareNetObjects;
    using Main.Constants;
    using Main.Models;
    using Main.Tests.TestData.Helper;
    using Models.Request.SecurityCode;
    using NSubstitute;
    using NUnit.Framework;
    using Params.SecurityCode;
    using SecurityCode.Web;
    using Services;
    using Site.Handlers.SecurityCode;
    using Site.ViewModels.Security;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using System;
    using Web.Helpers;

    [TestFixture]
    public class SecurityCodeHandlerTests
    {
        private SecurityCodeHandler securityCodeHandler;
        private ProfilePictureServices customerProfileService;
        private SessionManager sessionManager;
        private Encryptor encryptor;
        private SecurityCodeService securityCodeService;
        private IUserSession userSession;
        private IUser user;
        private CompareLogic compareLogic;
        private AesService aes;

        [SetUp]
        public void SetUp()
        {
            encryptor = Substitute.For<Encryptor>();
            encryptor.RandomString(Arg.Any<int>()).Returns("HashKey");

            sessionManager = Substitute.For<SessionManager>();
            sessionManager.Get<string>(Arg.Is("SECURITYCODETOKEN")).Returns("SecurityCode");

            customerProfileService = Substitute.For<ProfilePictureServices>();
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            securityCodeService = Substitute.For<SecurityCodeService>();
            securityCodeService.GenerateSecurityToken().Returns("SecurityCodeToken");
            securityCodeService.GenerateSalt(Arg.Any<int>()).Returns("SecuritySalt");

            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns(RoleId.Agent);

            user = Substitute.For<IUser>();
            user.IsAdminCanUpdateSubAccount.Returns(true);

            securityCodeHandler = new SecurityCodeHandler(
                securityCodeService,
                customerProfileService,
                sessionManager,
                encryptor,
                userSession,
                user,
                aes);

            compareLogic = new CompareLogic();            
            UrlHelperTestSetUp.SetUp();
            aes = Substitute.For<AesService>();
        }

        #region Handle Security Code Query

        [Test]
        public void HandleSecurityCodeQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            SecurityCodeQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleSecurityCodeQuery_MessageIsNotNull_CallsGenerateSecurityToken()
        {
            // Arrange
            var message = new SecurityCodeQuery(userSession);

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().GenerateSecurityToken();
        }

        [Test]
        public void HandleSecurityCodeQuery_MessageIsNotNull_CallsSessionManagerAdd()
        {
            // Arrange
            var message = new SecurityCodeQuery(userSession);

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            sessionManager.Received().Add(Arg.Is("SECURITYCODETOKEN"), Arg.Is("SecurityCodeToken"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void HandleSecurityCodeQuery_UserNameIsNullOrWhiteSpace_ReturnsSecurityCodeViewModel(string userName)
        {
            // Arrange
            sessionManager.Get<string>(Arg.Is("ph")).Returns("HashKey");

            var message = new SecurityCodeQuery(userSession);
            message.Username = userName;

            var expectedViewModel = new SecurityCodeViewModel
            {
                SecurityCodeToken = "SecurityCodeToken",
                SecuritySalt = "SecuritySalt",
                HashKey = "HashKey",
                CustRoleId = message.RoleId,
                Title = "Reset Security Code"
            };

            // Act
            var actualViewModel = securityCodeHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Test]
        public void HandleSecurityCodeQuery_UserNameIsNotNullOrWhiteSpace_ReturnsSecurityCodeViewModel()
        {
            // Arrange
            sessionManager.Get<string>(Arg.Is("ph")).Returns("HashKey");

            var message = new SecurityCodeQuery(userSession);
            message.Username = "UserName";

            var expectedViewModel = new SecurityCodeViewModel
            {
                SecurityCodeToken = "SecurityCodeToken",
                SecuritySalt = "SecuritySalt",
                HashKey = "HashKey",
                CustRoleId = message.RoleId,
                Title = "Reset Security Code - UserName"
            };

            // Act
            var actualViewModel = securityCodeHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        #endregion Handle Security Code Query

        #region Handle Force Change Security Code Query

        [Test]
        public void HandleForceChangeSecurityCodeQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ForceChangeSecurityCodeQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleForceChangeSecurityCodeQuery_MessageIsNotNull_CallsGetProfilePicture()
        {
            // Arrange
            var message = new ForceChangeSecurityCodeQuery(userSession);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(FakeDataHelper.Create<ActorProfilePictureModel>());

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            customerProfileService.Received().GetActorProfilePicture(Arg.Is<int>(custId => custId == userSession.CustId));
        }

        [Test]
        public void HandleForceChangeSecurityCodeQuery_MessageIsNotNull_CallsGenerateSecurityToken()
        {
            // Arrange
            var message = new ForceChangeSecurityCodeQuery(userSession);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(FakeDataHelper.Create<ActorProfilePictureModel>());

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().GenerateSecurityToken();
        }

        [Test]
        public void HandleForceChangeSecurityCodeQuery_MessageIsNotNull_CallsSessionManagerAdd()
        {
            // Arrange
            var message = new ForceChangeSecurityCodeQuery(userSession);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(FakeDataHelper.Create<ActorProfilePictureModel>());

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            sessionManager.Received().Add(Arg.Is("SECURITYCODETOKEN"), Arg.Is("SecurityCodeToken"));
        }

        [Test]
        public void HandleForceChangeSecurityCodeQuery_MessageIsNotNullAndUserIsMainAccount_ReturnsForceChangeSecurityCodeViewModel()
        {
            // Arrange
            userSession.IsSubAcc.Returns(false);
            userSession.NickName.Returns(string.Empty);
            userSession.UserName.Returns("UserName");
            var message = new ForceChangeSecurityCodeQuery(userSession);

            sessionManager.Get<string>(Arg.Is("ph")).Returns("HashKey");

            var profile = FakeDataHelper.Create<ActorProfilePictureModel>();
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(profile);

            var expectedViewModel = new ForceChangeSecurityCodeViewModel
            {
                SecurityCodeToken = "SecurityCodeToken",
                SecuritySalt = "SecuritySalt",
                HashKey = "HashKey",
                CustRoleId = message.RoleId,
                IsDefaultAvatar = true,
                Username = message.UserName,
                AlternateUsername = string.Empty,
                UsernameShower = "UserName",
                EmbedImage = string.Empty
            };

            var compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "OldMainRootPath", "MainRootPath", "EmbedImage" }
            });

            // Act
            var actualViewModel = securityCodeHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Test]
        public void HandleForceChangeSecurityCodeQuery_MessageIsNotNullAndUserIsSubAccount_ReturnsForceChangeSecurityCodeViewModel()
        {
            // Arrange
            userSession.IsSubAcc.Returns(true);
            userSession.SubUserName.Returns("SubUserName");
            var message = new ForceChangeSecurityCodeQuery(userSession);

            sessionManager.Get<string>(Arg.Is("ph")).Returns("HashKey");

            var profile = new ActorProfilePictureModel();
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(profile);

            var expectedViewModel = new ForceChangeSecurityCodeViewModel
            {
                SecurityCodeToken = "SecurityCodeToken",
                SecuritySalt = "SecuritySalt",
                HashKey = "HashKey",
                CustRoleId = message.RoleId,
                IsDefaultAvatar = false,
                Username = message.UserName,
                AlternateUsername = "(SUB-ACC SubUserName)",
                UsernameShower = "SubUserName",
                EmbedImage = string.Empty
            };

            var compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "OldMainRootPath", "MainRootPath" }
            });

            // Act
            var actualViewModel = securityCodeHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        #endregion Handle Force Change Security Code Query

        #region Handle Change Security Code First Login Command

        [Test]
        public void HandleChangeSecurityCodeFirstLoginCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeSecurityCodeFirstLoginCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleChangeSecurityCodeFirstLoginCommand_SecurityCodeIsNotNullOrWhiteSpace_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.SecurityCode.Returns("SecurityCode");
            var message = new ChangeSecurityCodeFirstLoginCommand(userSession);

            // Act
            var result = securityCodeHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void HandleChangeSecurityCodeFirstLoginCommand_SecurityCodeIsNullOrWhiteSpaceAndUserIsInternal_ReturnUnAuthorizedResult(string securityCode)
        {
            // Arrange
            userSession.SecurityCode.Returns(securityCode);
            userSession.IsInternal.Returns(true);
            var message = new ChangeSecurityCodeFirstLoginCommand(userSession);

            // Act
            var result = securityCodeHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleChangeSecurityCodeFirstLoginCommand_SubAccIdIsZero_CallsChangeMainAccountSecurityCode()
        {
            // Arrange
            userSession.SecurityCode.Returns(string.Empty);
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(0);
            var message = new ChangeSecurityCodeFirstLoginCommand(userSession);
            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 1 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SubAccId == message.SubAccId
                        && param.UserId == message.UserId
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.MustVerifyCurrentSecurityCode == false));
        }

        [Test]
        public void HandleChangeSecurityCodeFirstLoginCommand_SubAccIdIsNotZero_CallsChangeSubAccountSecurityCode()
        {
            // Arrange
            userSession.SecurityCode.Returns(string.Empty);
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(1);
            var message = new ChangeSecurityCodeFirstLoginCommand(userSession);
            securityCodeService.ChangeSubAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 1 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeSubAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SubAccId == message.SubAccId
                        && param.UserId == message.UserId
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.MustVerifyCurrentSecurityCode == false));
        }

        [Test]
        public void HandleChangeSecurityCodeFirstLoginCommand_ChangeSecurityCodeSuccessful_ReturnsSecurityCodeChanged()
        {
            // Arrange
            userSession.SecurityCode.Returns(string.Empty);
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(0);
            var message = new ChangeSecurityCodeFirstLoginCommand(userSession);
            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            var result = securityCodeHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsOk
                && result.Message == Security.alertSecCodeChangedOK);
        }

        #endregion Handle Change Security Code First Login Command

        #region Handle Change Security Code By Forcing Command

        [Test]
        public void HandleChangeSecurityCodeByForcingCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeSecurityCodeByForcingCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleChangeSecurityCodeByForcingCommand_UserIsInternal_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var message = new ChangeSecurityCodeByForcingCommand(userSession);

            // Act
            var result = securityCodeHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleChangeSecurityCodeByForcingCommand_SubAccIdIsZero_CallsChangeMainAccountSecurityCode()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(0);

            var message = new ChangeSecurityCodeByForcingCommand(userSession);
            message.SecurityCode = "NewSecurityCode";
            aes.Decrypt(Arg.Any<string>(), Arg.Is(message.SecurityCode)).Returns("DecryptedCode");

            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SubAccId == message.SubAccId
                        && param.UserId == message.UserId
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.CurrentSecurityCode == message.OldSecurityCode
                        && param.Salt == "SecurityCode"
                        && param.MustVerifyCurrentSecurityCode == true));
        }

        [Test]
        public void HandleChangeSecurityCodeByForcingCommand_SubAccIdIsNotZero_CallsChangeSubAccountSecurityCode()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(1);
            var message = new ChangeSecurityCodeByForcingCommand(userSession);
            securityCodeService.ChangeSubAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeSubAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SubAccId == message.SubAccId
                        && param.UserId == message.UserId
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.CurrentSecurityCode == message.OldSecurityCode
                        && param.Salt == "SecurityCode"
                        && param.MustVerifyCurrentSecurityCode == true));
        }

        #endregion Handle Change Security Code By Forcing Command

        #region Handle Change User Own Security Code Command

        [Test]
        public void HandleChangeUserOwnSecurityCodeCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeUserOwnSecurityCodeCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleChangeUserOwnSecurityCodeCommand_UserIsInternal_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var message = new ChangeUserOwnSecurityCodeCommand(userSession);

            // Act
            var result = securityCodeHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleChangeUserOwnSecurityCodeCommand_SubAccIdIsZero_CallsChangeMainAccountSecurityCode()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(0);
            var message = new ChangeUserOwnSecurityCodeCommand(userSession);
            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SubAccId == message.SubAccId
                        && param.UserId == message.UserId
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.CurrentSecurityCode == message.OldSecurityCode
                        && param.Salt == "SecurityCode"
                        && param.MustVerifyCurrentSecurityCode == true));
        }

        [Test]
        public void HandleChangeUserOwnSecurityCodeCommand_SubAccIdIsNotZero_CallsChangeSubAccountSecurityCode()
        {
            // Arrange
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(1);
            var message = new ChangeSecurityCodeByForcingCommand(userSession);
            securityCodeService.ChangeSubAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeSubAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SubAccId == message.SubAccId
                        && param.UserId == message.UserId
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.CurrentSecurityCode == message.OldSecurityCode
                        && param.Salt == "SecurityCode"
                        && param.MustVerifyCurrentSecurityCode == true));
        }

        #endregion Handle Change User Own Security Code Command

        #region Handle Reset Security Code Command

        [Test]
        public void HandleResetSecurityCodeCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetSecurityCodeCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleResetSecurityCodeCommand_InternalAccountAndSubAccIdIsGreaterThanZero_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.SecurityCode.Returns("SecurityCode");
            userSession.IsInternal.Returns(true);

            var message = new ResetSecurityCodeCommand(userSession);
            message.SubAccId = 0;

            // Act
            var result = securityCodeHandler.Handle(message);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleResetSecurityCodeCommand_CustIdIsGreaterThanZero_CallsChangeMainAccountSecurityCode()
        {
            // Arrange
            userSession.IsInternal.Returns(false);

            var message = new ResetSecurityCodeCommand(userSession);
            message.CustId = 1;

            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    param => param.CustId == message.CustId.Value
                        && param.UserId == userSession.UserId
                        && param.SubAccId == userSession.SubAccId
                        && param.IsResetMember == true
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP
                        && param.MustVerifyCurrentSecurityCode == false));
        }

        [Test]
        public void HandleResetSecurityCodeCommand_SubAccIdIsGreaterThanZero_CallsResetSubAccountSecurityCode()
        {
            // Arrange
            userSession.IsInternal.Returns(false);

            var message = new ResetSecurityCodeCommand(userSession);
            message.SubAccId = 1;

            securityCodeService.ResetSubAccountSecurityCode(Arg.Any<ResetSecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().ResetSubAccountSecurityCode(
                Arg.Is<ResetSecurityCodeParam>(
                    param => param.CustId == userSession.CustId
                        && param.UserId == userSession.UserId
                        && param.SubAccId == message.SubAccId.Value
                        && param.NewSecurityCode == message.SecurityCode
                        && param.Language == message.Language
                        && param.IP == message.IP));
        }

        #endregion Handle Reset Security Code Command

        #region Handle Verify Security Code Command

        [Test]
        public void HandleVerifySecurityCodeCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            VerifySecurityCodeCommand message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeHandler.Handle(message));
        }

        [Test]
        public void HandleVerifySecurityCodeCommand_MessageIsNotNull_CallsVerifySecurityCode()
        {
            // Arrange
            var message = new VerifySecurityCodeCommand(userSession);

            securityCodeService.VerifySecurityCode(Arg.Any<VerifySecurityCodeParam>()).Returns(new Result { Code = 0 });

            // Act
            securityCodeHandler.Handle(message);

            // Assert
            securityCodeService.Received().VerifySecurityCode(
                Arg.Is<VerifySecurityCodeParam>(
                    param => param.CustId == message.CustId
                        && param.SecurityCode == message.SecurityCode
                        && param.IP == message.IP
                        && param.Language == message.Language
                        && param.Salt == "SecurityCode"
                        && param.SubAccId == message.SubAccId));
        }

        #endregion Handle Verify Security Code Command
    }
}