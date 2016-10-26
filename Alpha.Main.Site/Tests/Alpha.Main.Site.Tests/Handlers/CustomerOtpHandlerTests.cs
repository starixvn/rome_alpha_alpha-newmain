namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using AppResources;
    using Helpers;
    using Main.Models;
    using Main.Models.Otp;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Otp;
    using Services;
    using Site.Handlers;
    using Site.Requests.Otp;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class CustomerOtpHandlerTests
    {
        private CustomerOtpHandler customerOtpHandler;
        private CustomerOtpService customerOtpService;
        private SessionManager sessionManager;
        private IUserSession userSession;
        private CustomerProfileService customerProfileService;
        private BrowserHelper browserHelper;

        [SetUp]
        public void SetUp()
        {
            customerOtpService = Substitute.For<CustomerOtpService>();
            userSession = Substitute.For<IUserSession>();
            sessionManager = Substitute.For<SessionManager>();
            customerProfileService = Substitute.For<CustomerProfileService>();
            browserHelper = Substitute.For<BrowserHelper>();
            customerOtpHandler = new CustomerOtpHandler(customerOtpService, userSession, sessionManager, customerProfileService, browserHelper);
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

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().ChangeOtpStatus(
                Arg.Is<ChangeOtpStatusParam>(
                    param => param.CustId == command.CustId
                        && param.ActorId == command.CustId
                        && param.SubAccountId == command.SubAccountId
                        && param.IP == command.IP
                        && param.Language == command.Language
                        && param.IsEnable == command.IsEnable));
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
        public void HandleResetOtpCommand_AdminResetsSubAccountOtp_CallRemoveSecretKey()
        {
            // Arrange
            userSession.IsInternal.Returns(true);
            var command = new ResetOtpCommand(userSession);
            command.SubAccountId = 3;
            command.CustId = 3;

            // Act
            var result = customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().RemoveSecretKey(Arg.Is<RemoveSecretKeyParam>(
                param => param.IsDisableOtp
                    && param.CustId == command.CustId
                    && param.SubAccountId == command.SubAccountId
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

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().RemoveSecretKey(
                Arg.Is<RemoveSecretKeyParam>(
                    param => param.CustId == command.CustId
                        && param.ActorId == command.ActorId
                        && param.SubAccountId == command.SubAccountId
                        && param.IP == command.IP
                        && param.Language == command.Language));
        }

        [Test]
        public void HandleResetOtpCommand_OtpStatusChanged_ReturnResult()
        {
            // Arrange
            var command = new ResetOtpCommand(userSession);
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

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.RemoveSecretKey(Arg.Is<RemoveSecretKeyParam>(
                param => param.CustId == 1
                    && param.SubAccountId == 2
                    && !param.IsDisableOtp
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

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            browserHelper.Received().GetDeviceInfo();
        }

        [Test]
        public void HandleSetUpOtpCommand_CallSetUpOtp()
        {
            // Arrange
            var command = new SetUpOtpCommand(userSession);
            var expectedDeviceInfo = new DeviceInfo
            {
                IP = "IP",
                DeviceName = "DeviceName",
                BrowserName = "BrowserName",
                BrowserVersion = "BrowserVersion",
                DeviceVersion = "DeviceVersion"
            };
            browserHelper.GetDeviceInfo().Returns(expectedDeviceInfo);
            var expectedSecretKey = "expectedSecretKey";
            sessionManager.Get<string>(Arg.Any<string>()).Returns(expectedSecretKey);

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().SetUpOtp(Arg.Is<CustomerOtp>(
                customerOtp => customerOtp.CustID == command.CustId
                            && customerOtp.SubAccId == command.SubAccountId
                            && customerOtp.SecretKey == expectedSecretKey
                            && customerOtp.DeviceInfo.IP == expectedDeviceInfo.IP
                            && customerOtp.DeviceInfo.DeviceName == expectedDeviceInfo.DeviceName
                            && customerOtp.DeviceInfo.BrowserName == expectedDeviceInfo.BrowserName
                            && customerOtp.DeviceInfo.BrowserVersion == expectedDeviceInfo.BrowserVersion
                            && customerOtp.DeviceInfo.DeviceVersion == expectedDeviceInfo.DeviceVersion));
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
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp { Enabled = false });
            customerOtpService.GenerateCustomerOtp(1, 2).Returns(new CustomerOtp());

            // Act
            customerOtpHandler.Handle(setUpOtpQuery);

            // Assert
            customerOtpService.Received().GenerateCustomerOtp(1, 2);
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
                CustID = 1,
                Enabled = true,
                QrCode = "QrCode",
                SubAccId = 2,
                DeviceInfo = new DeviceInfo
                {
                    IP = "1"
                },
                SecretKey = "SecretKey",
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedCustomerOtp);

            // Act
            var actualCustomerOtp = customerOtpHandler.Handle(setUpOtpQuery);

            // Assert
            Assert.IsTrue(expectedCustomerOtp.CustID == actualCustomerOtp.CustomerOtp.CustID
                && expectedCustomerOtp.SubAccId == actualCustomerOtp.CustomerOtp.SubAccId
                && expectedCustomerOtp.QrCode == actualCustomerOtp.CustomerOtp.QrCode
                && expectedCustomerOtp.SecretKey == actualCustomerOtp.CustomerOtp.SecretKey
                && expectedCustomerOtp.DeviceInfo.IP == actualCustomerOtp.CustomerOtp.DeviceInfo.IP
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

            // Act
            customerOtpHandler.Handle(command);

            // Assert
            customerOtpService.Received().VerifyOtp(Arg.Is<CustomerOtp>(customerOtp
                => customerOtp.CustID == command.CustId
                    && customerOtp.SubAccId == command.SubAccountId
                    && customerOtp.Otp == command.Otp));
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
    }
}