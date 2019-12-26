namespace Alpha.Main.Site.Tests.Handlers
{
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Services;
    using AppResources;
    using Main.Models;
    using Main.Models.Otp;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Otp;
    using SecurityCode.Web;
    using Services;
    using Site.Handlers;
    using Site.Helpers;
    using Site.Requests.Otp;
    using Sunplus.Agent.Model.Entities;
    using System;
    using Web.Helpers;

    [TestFixture]
    public class CustomerOtpHandlerTests
    {
        private CustomerOtpHandler customerOtpHandler;
        private CustomerOtpService customerOtpService;
        private SessionManager sessionManager;
        private IUserSession userSession;
        private ProfilePictureServices customerProfileService;
        private BrowserHelper browserHelper;
        private IUser user;
        private NicknameChecker nicknameChecker;

        [SetUp]
        public void SetUp()
        {
            customerOtpService = Substitute.For<CustomerOtpService>();
            userSession = Substitute.For<IUserSession>();
            sessionManager = Substitute.For<SessionManager>();
            customerProfileService = Substitute.For<ProfilePictureServices>();
            browserHelper = Substitute.For<BrowserHelper>();
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            user = Substitute.For<IUser>();
            user.IsAdminCanUpdateSubAccount.Returns(true);

            nicknameChecker = Substitute.For<NicknameChecker>();

            customerOtpHandler = new CustomerOtpHandler(customerOtpService, userSession, sessionManager, browserHelper, user, nicknameChecker);
        }

        #region Handle Change Otp Status Command

        [Test]
        public void HandleChangeOtpStatusCommand_MessageIsNull_ThrowArgumentNullException()
        {
            ChangeOtpStatusCommand message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleChangeOtpStatusCommand_UserIsInternalAdmin_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new ChangeOtpStatusCommand(userSession);

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.IsNotOk
                && result.Message == Security.NotAuthorized);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HandleChangeOtpStatusCommand_CallChangeOtpStatus(bool expectedIsEnable)
        {
            // Arrange
            var command = new ChangeOtpStatusCommand(userSession);
            command.IsEnable = expectedIsEnable;
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            userSession.UserLoginIp.Returns("ip");
            userSession.Language.Returns("language");

            customerOtpService.ChangeOtpStatus(Arg.Any<ChangeOtpStatusParam>()).Returns(new Result { Code = 1 });

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().ChangeOtpStatus(
                Arg.Is<ChangeOtpStatusParam>(
                    param => param.CustId == command.CustId
                        && param.ActorId == command.CustId
                        && param.SubAccId == command.SubAccountId
                        && param.IP == command.IP
                        && param.Language == command.Language
                        && param.IsEnable == command.IsEnable));
        }

        [Test]
        public void HandleChangeOtpStatusCommand_OtpStatusChanged_SetOtpStatusToSession()
        {
            // Arrange
            var command = new ChangeOtpStatusCommand(userSession);
            command.IsEnable = true;
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            userSession.UserLoginIp.Returns("ip");
            userSession.Language.Returns("language");

            customerOtpService.ChangeOtpStatus(Arg.Any<ChangeOtpStatusParam>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            SecurityCodeSessionManager.SecurityCodeSession.Received().IsEnabledOtp = true;
        }

        [Test]
        public void HandleChangeOtpStatusCommand_OtpStatusChanged_ReturnResult()
        {
            // Arrange
            var command = new ChangeOtpStatusCommand(userSession);
            customerOtpService.ChangeOtpStatus(Arg.Any<ChangeOtpStatusParam>())
                .Returns(new Result
                {
                    Code = 1,
                    Message = "Changed"
                });

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == "Changed");
        }

        #endregion Handle Change Otp Status Command

        #region Handle Reset Otp Command Command

        [Test]
        public void HandleResetOtpCommand_MessageIsNull_ThrowArgumentNullException()
        {
            ResetOtpCommand message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleResetOtpCommand_AdminResetsMainAccountOtp_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new ResetOtpCommand(userSession);

            // Act
            var viewModel = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(viewModel.Result.IsNotOk
                && viewModel.Result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleResetOtpCommand_SubAccountHasLimitedHostPermision_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsLimitedHostPermisionOnSubAccountFunction.Returns(true);
            var command = new ResetOtpCommand(userSession);

            // Act
            var viewModel = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(viewModel.Result.IsNotOk
                && viewModel.Result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleResetOtpCommand_SubAccountDoesHasHostPermissionOnUpdate_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsSubAcc.Returns(true);
            userSession.SubPageAccess.Returns(new PageAccess
            {
                UpdateAccount = false
            });
            var command = new ResetOtpCommand(userSession);

            // Act
            var viewModel = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(viewModel.Result.IsNotOk
                && viewModel.Result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleResetOtpCommand_UserHasPermission_CallGetDeviceInfo()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new ResetOtpCommand(userSession);
            command.SubAccountId = 3;
            command.CustId = 3;
            browserHelper.GetDevice().Returns(new Device { Info = new DeviceInfo() });

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            browserHelper.Received().GetDevice();
        }

        [Test]
        public void HandleResetOtpCommand_AdminResetsSubAccountOtp_CallRemoveSecretKey()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new ResetOtpCommand(userSession);
            command.SubAccountId = 3;
            command.CustId = 3;
            browserHelper.GetDevice().Returns(new Device { Info = new DeviceInfo() });

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().RemoveSecretKey(Arg.Is<RemoveSecretKeyParam>(
                param => param.CustId == command.CustId
                    && param.SubAccId == command.SubAccountId
                    && param.ActorId == command.ActorId));
        }

        [Test]
        public void HandleResetOtpCommand_CallRemoveSecretKey()
        {
            // Arrange
            var command = new ResetOtpCommand(userSession);
            command.CustId = 1;
            command.SubAccountId = 2;
            userSession.UserLoginIp.Returns("ip");
            userSession.Language.Returns("language");
            browserHelper.GetDevice().Returns(new Device { Info = new DeviceInfo { IP = "10" } });

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().RemoveSecretKey(
                Arg.Is<RemoveSecretKeyParam>(
                    param => param.CustId == command.CustId
                        && param.ActorId == command.ActorId
                        && param.SubAccId == command.SubAccountId
                        && param.IP == "10"
                        && param.Language == command.Language));
        }

        [Test]
        public void HandleResetOtpCommand_OtpStatusChanged_ReturnResult()
        {
            // Arrange
            var command = new ResetOtpCommand(userSession);
            browserHelper.GetDevice().Returns(new Device { Info = new DeviceInfo() });
            customerOtpService.RemoveSecretKey(Arg.Any<RemoveSecretKeyParam>())
                .Returns(new Result
                {
                    Code = 1,
                    Message = "Changed"
                });

            // Act
            var viewModel = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(viewModel.Result.Code == 1 && viewModel.Result.Message == "Changed");
        }

        #endregion Handle Reset Otp Command Command

        #region Reset Otp Query

        [Test]
        public void HandleResetOtpQuery_MessageIsNull_ThrowArgumentNullException()
        {
            ResetOtpQuery message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleResetOtpQuery_CreateResetOtpViewModel()
        {
            // Arrange
            var query = new ResetOtpQuery
            {
                CustId = 1,
                SubAccountId = 2
            };
            userSession.CustId.Returns(1);

            // Act
            var viewModel = customerOtpHandler.Handle(query);

            // Assert
            Assert.IsTrue(viewModel.CustId == query.CustId
                && viewModel.SubAccountId == query.SubAccountId);
        }

        #endregion Reset Otp Query

        #region Reset Otp Query

        [Test]
        public void HandleRemoveOtpCommand_MessageIsNull_ThrowArgumentNullException()
        {
            RemoveOtpCommand message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleRemoveOtpCommand_CallRemoveSecretKey()
        {
            // Arrange
            var command = new RemoveOtpCommand(userSession);
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            userSession.Language.Returns("ee");
            userSession.UserLoginIp.Returns("4");
            browserHelper.GetDevice().Returns(new Device { Info = new DeviceInfo() });

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.RemoveSecretKey(Arg.Is<RemoveSecretKeyParam>(
                param => param.CustId == 1
                    && param.SubAccId == 2
                    && param.ActorId == 1
                    && param.Language == "ee"
                    && param.IP == "4"));
        }

        [Test]
        public void HandleRemoveOtpCommand_UserIsInternalAdmin_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new RemoveOtpCommand(userSession);

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.IsNotOk && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleRemoveOtpCommand_DidRemoveCommand_ReturnResult()
        {
            // Arrange
            var command = new RemoveOtpCommand(userSession);
            customerOtpService.RemoveSecretKey(Arg.Any<RemoveSecretKeyParam>())
                .Returns(new Result
                {
                    Code = 1,
                    Message = "Changed"
                });
            browserHelper.GetDevice().Returns(new Device { Info = new DeviceInfo() });

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == "Changed");
        }

        #endregion Reset Otp Query

        #region Handle Change Otp Status Command

        [Test]
        public void HandleSetUpOtpCommand_MessageIsNull_ThrowArgumentNullException()
        {
            SetUpOtpCommand message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleSetUpOtpCommand_UserIsInternalAdmin_ReturnUnAuthorizedResult()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new SetUpOtpCommand(userSession);

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.IsNotOk && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void HandleSetUpOtpCommand_GetSecretKeyFromSession_CallSessonManagerGet()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);
            customerOtpService.SetUpOtp(Arg.Any<CustomerOtp>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            sessionManager.Received().Get<string>("CustomerOtpHandler.SecretKeySessionKey");
        }

        [Test]
        public void HandleSetUpOtpCommand_CallGetDeviceInfo()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);
            customerOtpService.SetUpOtp(Arg.Any<CustomerOtp>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            browserHelper.Received().GetDevice();
        }

        [Test]
        public void HandleSetUpOtpCommand_CallSetUpOtp()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);
            var expectedDeviceInfo = new Device
            {
                Info = new DeviceInfo
                {
                    IP = "IP",
                    DeviceName = "DeviceName",
                    BrowserName = "BrowserName",
                    BrowserVersion = "BrowserVersion",
                    DeviceVersion = "DeviceVersion"
                }
            };
            browserHelper.GetDevice().Returns(expectedDeviceInfo);
            var expectedSecretKey = "expectedSecretKey";
            sessionManager.Get<string>(Arg.Any<string>()).Returns(expectedSecretKey);
            customerOtpService.SetUpOtp(Arg.Any<CustomerOtp>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().SetUpOtp(Arg.Is<CustomerOtp>(
                customerOtp => customerOtp.CustId == command.CustId
                            && customerOtp.SubAccId == command.SubAccountId
                            && customerOtp.SecretKey == expectedSecretKey
                            && customerOtp.Device.Info.IP == expectedDeviceInfo.Info.IP
                            && customerOtp.Device.Info.DeviceName == expectedDeviceInfo.Info.DeviceName
                            && customerOtp.Device.Info.BrowserName == expectedDeviceInfo.Info.BrowserName
                            && customerOtp.Device.Info.BrowserVersion == expectedDeviceInfo.Info.BrowserVersion
                            && customerOtp.Device.Info.DeviceVersion == expectedDeviceInfo.Info.DeviceVersion));
        }

        [Test]
        public void HandleSetUpOtpCommand_DidSetUpOtp_SetVerifiedOtpToSession()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);
            customerOtpService.SetUpOtp(Arg.Any<CustomerOtp>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            SecurityCodeSessionManager.SecurityCodeSession.Received().IsAlreadyInputSecurityCode = true;
        }

        [Test]
        public void HandleSetUpOtpCommand_DidSetUpOtp_ReturnResult()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);
            var expectedResult = new Result
            {
                Code = 3,
                Message = "Message"
            };
            customerOtpService.SetUpOtp(Arg.Any<CustomerOtp>()).Returns(expectedResult);

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(result.Code == expectedResult.Code
                && result.Message == expectedResult.Message);
        }

        #endregion Handle Change Otp Status Command

        #region Handle Set Up Otp Query

        [Test]
        public void HandleSetUpOtpQuery_MessageIsNull_ThrowArgumentNullException()
        {
            SetUpOtpQuery message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleSetupOtpQuery_CheckWhetherCustomerHasOtp_CallGetCustomerOtp()
        {
            // Arrange
            var setUpOtpQuery = new SetUpOtpQuery(userSession);
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new CustomerOtp { Enabled = true });

            // Act
            customerOtpHandler.Handle(setUpOtpQuery);

            // Assert
            customerOtpService.Received().GetCustomerOtp(1, 2);
        }

        [Test]
        public void HandleSetUpOtpQuery_CustomerHasNotSetUpYet_CallGenerateCustomerOtp()
        {
            // Arrange
            var setUpOtpQuery = new SetUpOtpQuery(userSession);
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            userSession.IsSubAcc.Returns(true);
            userSession.SubUserName.Returns("a");
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp { Enabled = false });
            customerOtpService.GenerateCustomerOtp(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>())
                .Returns(new CustomerOtp());

            // Act
            customerOtpHandler.Handle(setUpOtpQuery);

            // Assert
            customerOtpService.Received().GenerateCustomerOtp(1, "a", 2);
        }

        [Test]
        public void HandleSetUpOtpQuery_CustomerDidSetUpOtp_ReturnCustomerOtp()
        {
            // Arrange
            var setUpOtpQuery = new SetUpOtpQuery(userSession);
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            var expectedCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                Enabled = true,
                QRCode = "QrCode",
                SubAccId = 2,
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "1"
                    }
                },
                SecretKey = "SecretKey",
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedCustomerOtp);

            // Act
            var actualCustomerOtp = customerOtpHandler.Handle(setUpOtpQuery);

            // Assert
            Assert.IsTrue(expectedCustomerOtp.CustId == actualCustomerOtp.CustomerOtp.CustId
                && expectedCustomerOtp.SubAccId == actualCustomerOtp.CustomerOtp.SubAccId
                && expectedCustomerOtp.QRCode == actualCustomerOtp.CustomerOtp.QRCode
                && expectedCustomerOtp.SecretKey == actualCustomerOtp.CustomerOtp.SecretKey
                && expectedCustomerOtp.Device.Info.IP == actualCustomerOtp.CustomerOtp.Device.Info.IP
                && expectedCustomerOtp.Enabled);
        }

        #endregion Handle Set Up Otp Query

        #region Verify Otp Command

        [Test]
        public void HandleVerifyOtpCommand_MessageIsNull_ThrowArgumentNullException()
        {
            VerifyOtpCommand message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleVerifyOtpCommand_CallVerifyOtp()
        {
            // Arrange
            var command = new VerifyOtpCommand(userSession);
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            command.Otp = "Otp";
            customerOtpService.VerifyOtp(Arg.Any<CustomerOtp>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().VerifyOtp(Arg.Is<CustomerOtp>(customerOtp
                => customerOtp.CustId == command.CustId
                    && customerOtp.SubAccId == command.SubAccountId
                    && customerOtp.Otp == command.Otp));
        }

        [Test]
        public void HandleVerifyOtpCommand_DidVerifyOtp_SetVerifiedOtpToSession()
        {
            // Arrange
            var command = new VerifyOtpCommand(userSession);
            userSession.CustId.Returns(1);
            userSession.SubAccId.Returns(2);
            command.Otp = "Otp";
            customerOtpService.VerifyOtp(Arg.Any<CustomerOtp>()).Returns(new Result());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            SecurityCodeSessionManager.SecurityCodeSession.Received().IsAlreadyInputSecurityCode = true;
        }

        [Test]
        public void HandleVerifyOtpCommand_DidVerifyOtp_ReturnResult()
        {
            // Arrange
            var command = new VerifyOtpCommand(userSession);
            var expectedResult = new Result
            {
                Code = 3,
                Message = "Message"
            };
            customerOtpService.VerifyOtp(Arg.Any<CustomerOtp>()).Returns(expectedResult);

            // Act
            var verifyResult = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsTrue(verifyResult.Code == expectedResult.Code
                && verifyResult.Message == expectedResult.Message);
        }

        #endregion Verify Otp Command

        #region Handle Verify Otp Different Device Query

        [Test]
        public void HandleVerifyOtpDifferentDeviceQuery_MessageIsNull_ThrowArgumentNullException()
        {
            VerifyOtpDifferentDeviceQuery message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleVerifyOtpDifferentDeviceQuery_UserIsSubAcc_ReturnFormattedSubUsername()
        {
            // Arrange
            var query = new VerifyOtpDifferentDeviceQuery(userSession);
            var subUsername = "subUsername";
            userSession.IsSubAcc.Returns(true);
            userSession.SubUserName.Returns(subUsername);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            // Act
            var viewModel = customerOtpHandler.Handle(query);

            // Assert
            Assert.AreEqual("(SUB-ACC subUsername)", viewModel.AlternateUsername);
        }

        [Test]
        public void HandleVerifyOtpDifferentDeviceQuery_UserHasNickname_ReturnFormattedNickname()
        {
            // Arrange
            var query = new VerifyOtpDifferentDeviceQuery(userSession);
            var nickname = "nickname";
            userSession.NickName.Returns(nickname);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            // Act
            var viewModel = customerOtpHandler.Handle(query);

            // Assert
            Assert.AreEqual("(nickname)", viewModel.AlternateUsername);
        }

        [Test]
        public void HandleVerifyOtpDifferentDeviceQuery_UserDoesNotHaveNickname_ReturnEmptyAltenativeUsername()
        {
            // Arrange
            var query = new VerifyOtpDifferentDeviceQuery(userSession);
            customerProfileService.GetActorProfilePicture(Arg.Any<int>()).Returns(new ActorProfilePictureModel());

            // Act
            var viewModel = customerOtpHandler.Handle(query);

            // Assert
            Assert.IsEmpty(viewModel.AlternateUsername);
        }
        #endregion Handle Verify Otp Different Device Query

        #region Check Difference Device Command

        [Test]
        public void HandleCheckDifferenceDeviceCommand_MessageIsNull_ThrowArgumentNullException()
        {
            CheckDifferenceDeviceCommand message = null;

            Assert.Throws<ArgumentNullException>(() => customerOtpHandler.Handle(message));
        }

        [Test]
        public void HandleCheckDifferenceDeviceCommand_CallGetCustomerOtp()
        {
            // Arrange
            var command = new CheckDifferenceDeviceCommand(this.userSession);
            userSession.CustId.Returns(10);
            userSession.SubAccId.Returns(20);

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().IsEnabledOtp(10, 20);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HandleCheckDifferenceDeviceCommand_GotCustomerOtp_SetOtpStatusToSession(bool otpStatus)
        {
            // Arrange
            var command = new CheckDifferenceDeviceCommand(this.userSession);

            customerOtpService.IsEnabledOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(otpStatus);
            browserHelper.GetDevice().Returns(new Device());

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            SecurityCodeSessionManager.SecurityCodeSession.Received().IsEnabledOtp = otpStatus;
        }

        [Test]
        public void HandleCheckDifferenceDeviceCommand_UserDoesNotEnableOtp_ReturnFalse()
        {
            // Arrange
            var command = new CheckDifferenceDeviceCommand(this.userSession);

            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp { Enabled = false, Device = new Device() });
            browserHelper.GetDevice().Returns(new Device());

            // Act
            var isDifferenceDevice = customerOtpHandler.Handle(command);

            // Assert
            Assert.IsFalse(isDifferenceDevice);
        }

        #endregion Check Difference Device Command
    }
}