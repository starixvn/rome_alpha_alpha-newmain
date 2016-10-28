namespace Alpha.Main.Tests.Services
{
    using System;
    using AppResources;
    using Criteria.Otp;
    using Fanex.Data.Repository;
    using Main.Helpers;
    using Main.Models;
    using Main.Models.Otp;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.OTP.Services;
    using Params.Otp;
    using Sunplus.Agent.Model.Casino;

    [TestFixture]
    public class CustomerOtpServiceImplTests
    {
        private CustomerOtpServiceImpl customerOtpService;
        private IDynamicRepository dynamicRepository;
        private OtpService otpService;
        private JsonHelper jsonHelper;
        private CasinoService casinoService;
        private IDateTime dateTime;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            otpService = Substitute.For<OtpService>();
            jsonHelper = Substitute.For<JsonHelper>();
            casinoService = Substitute.For<CasinoService>();
            dateTime = Substitute.For<IDateTime>();
            customerOtpService = Substitute.ForPartsOf<CustomerOtpServiceImpl>(dynamicRepository, otpService, jsonHelper, casinoService, dateTime);
        }

        #region Change Otp Status

        [Test]
        public void ChangeOtpStatus_ChangeOtpStatusParamIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => customerOtpService.ChangeOtpStatus(null));
        }

        [Test]
        public void ChangeOtpStatus_CallExecuteUpdateCustomerOtpStatusCommand()
        {
            // Arrange
            var param = new ChangeOtpStatusParam
            {
                ActorId = 1,
                CustId = 2,
                SubAccountId = 3,
                IP = "192.168.1.1",
                Language = "E"
            };

            // Act
            customerOtpService.ChangeOtpStatus(param);

            // Assert
            dynamicRepository.Received().Execute(
                Arg.Is<UpdateCustomerOtpStatusCommand>(
                    command => command.Language == param.Language
                                && command.IP == param.IP
                                && command.CustID == param.CustId
                                && command.SubAccID == param.SubAccountId
                                && command.UserID == param.ActorId
                                && command.Enable == param.IsEnable));
        }

        [Test]
        public void ChangeOtpStatus_ExecuteUpdateCustomerOtpStatusCommand_ReturnResult()
        {
            // Arrange
            var param = new ChangeOtpStatusParam();
            var expectedResult = new Result
            {
                Code = 1,
                Message = "Message"
            };
            dynamicRepository
                .When(repo => repo.Execute(Arg.Any<UpdateCustomerOtpStatusCommand>()))
                .Do((callInfo) =>
                {
                    var command = callInfo[0] as UpdateCustomerOtpStatusCommand;
                    command.ErrCode = expectedResult.Code;
                    command.ErrMsg = expectedResult.Message;
                });

            // Act
            var actualResult = customerOtpService.ChangeOtpStatus(param);

            // Assert
            Assert.IsTrue(actualResult.Code == expectedResult.Code
                && actualResult.Message == expectedResult.Message);
        }

        #endregion Change Otp Status

        #region Generate Customer Otp

        [TestCase(0)]
        [TestCase(-1)]
        public void GenerateCustomerOtp_CustIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int custId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.GenerateCustomerOtp(custId, "username"));
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void GenerateCustomerOtp_SubAccountIdCannotBeNegative_ThrowArgumentOutOfRangeException(int subAccountId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.GenerateCustomerOtp(1, "username", subAccountId));
        }

        [Test]
        public void GenerateCustomerOtp_ExecuteGenerateSecretKey()
        {
            // Arrange
            var secretKeyLength = 20; 

            // Act
            customerOtpService.GenerateCustomerOtp(10, "username");

            // Assert
            otpService.Received().GenerateSecretKey(secretKeyLength);
        }

        [Test]
        public void GenerateCustomerOtp_ExecuteGenerateQrCode()
        {
            // Arrange
            var secretKey = "SecretKey";
            otpService.GenerateSecretKey(20).Returns(secretKey);

            // Act
            customerOtpService.GenerateCustomerOtp(10, "username");

            // Assert
            otpService.Received().GenerateQrCode(secretKey, 30, 6, "MyAppName", "username");
        }

        [Test]
        public void GenerateCustomerOtp_ReturnCustomerOtp()
        {
            // Arrange
            var secretKey = "SecretKey";
            otpService.GenerateSecretKey(length: 20).Returns(secretKey);
            var quickResponseCode = "qrCode";
            otpService.GenerateQrCode(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(quickResponseCode);
            var custId = 1;
            var subAccountId = 1;

            // Act
            var actualCustomerOtp = customerOtpService.GenerateCustomerOtp(custId, "username", subAccountId);

            // Assert
            Assert.IsTrue(actualCustomerOtp.CustId == custId
                && actualCustomerOtp.SubAccId == subAccountId
                && actualCustomerOtp.QrCode == quickResponseCode
                && actualCustomerOtp.SecretKey == secretKey
                && !actualCustomerOtp.Enabled);
        }

        #endregion Generate Customer Otp

        #region Is Enabled Otp

        [TestCase(0)]
        [TestCase(-1)]
        public void IsEnabledOtp_CustIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int custId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.IsEnabledOtp(custId));
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void IsEnabledOtp_SubAccountIdCannotBeNegative_ThrowArgumentOutOfRangeException(int subAccountId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.IsEnabledOtp(1, subAccountId));
        }

        [Test]
        public void IsEnableOtp_GetFromDatabase_CallExecuteCheckEnableOtpCommand()
        {
            // Arrange
            var custId = 1;
            var subAccId = 2;

            // Act
            customerOtpService.IsEnabledOtp(custId, subAccId);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<CheckEnableOtpCommand>(
                criteria => criteria.CustID == custId
                        && criteria.SubAccID == subAccId));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsEnableOtp_ExecuteCheckEnableOtpCommand_ReturnEnable(bool isEnable)
        {
            // Arrange
            dynamicRepository
                .When(repo => repo.Execute(Arg.Any<CheckEnableOtpCommand>()))
                .Do((callInfo) =>
                {
                    var command = callInfo[0] as CheckEnableOtpCommand;
                    command.Enable = isEnable;
                });

            // Act
            var actualResult = customerOtpService.IsEnabledOtp(1, 2);

            // Assert
            Assert.AreEqual(isEnable, actualResult);
        }

        #endregion Is Enabled Otp

        #region Set Up Otp

        [Test]
        public void SetUpOtp_CustomerOtpIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => customerOtpService.SetUpOtp(null));
        }

        [Test]
        public void SetUpOtp_CurrentOtpDoesNotMatchWithSecretKey_ReturnErrorResult()
        {
            // Arrange
            var customerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Enabled = true,
                Otp = "333",
                QrCode = "aaa",
                SecretKey = "4545",
                Language = "E"
            };

            otpService.VerifyOtp(customerOtp.SecretKey, customerOtp.Otp, 30, 6).Returns(false);

            // Act
            var result = customerOtpService.SetUpOtp(customerOtp);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void SetUpOtp_SerializeDeviceInfoToJsonString()
        {
            // Arrange
            var customerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Enabled = true,
                DeviceInfo = new DeviceInfo
                {
                    IP = "2323232",
                    BrowserName = "BrowserName",
                    BrowserVersion = "BrowserVersion",
                    DeviceName = "DeviceName",
                    DeviceVersion = "DeviceVersion"
                },
                QrCode = "aaa",
                SecretKey = "4545",
                Language = "E"
            };
            otpService.VerifyOtp(customerOtp.SecretKey, customerOtp.Otp, 30, 6).Returns(true);

            // Act
            customerOtpService.SetUpOtp(customerOtp);

            // Assert
            jsonHelper.Received().Serialize(Arg.Is<DeviceInfo>(deviceInfo =>
                deviceInfo.IP == customerOtp.DeviceInfo.IP
                && deviceInfo.BrowserName == customerOtp.DeviceInfo.BrowserName
                && deviceInfo.BrowserVersion == customerOtp.DeviceInfo.BrowserVersion
                && deviceInfo.DeviceName == customerOtp.DeviceInfo.DeviceName
                && deviceInfo.DeviceVersion == customerOtp.DeviceInfo.DeviceVersion));
        }

        [Test]
        public void SetUpOtp_CustomerIsMainAccount_CallExecuteInsertCustomerOtpCommand()
        {
            // Arrange
            var customerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                Enabled = true,
                DeviceInfo = new DeviceInfo
                {
                    IP = "2323232",
                    BrowserName = "BrowserName",
                    BrowserVersion = "BrowserVersion",
                    DeviceName = "DeviceName",
                    DeviceVersion = "DeviceVersion"
                },
                QrCode = "aaa",
                SecretKey = "4545",
                Language = "E"
            };
            otpService.VerifyOtp(customerOtp.SecretKey, customerOtp.Otp, 30, 6).Returns(true);
            var deviceInfoSerialized = "deviceInfoSerialized";
            jsonHelper.Serialize(Arg.Any<object>()).Returns(deviceInfoSerialized);

            // Act
            customerOtpService.SetUpOtp(customerOtp);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<InsertCustomerOtpCommand>(
                command => command.CustID == customerOtp.CustId
                && command.SubAccID == customerOtp.SubAccId
                && command.SecretKey == customerOtp.SecretKey
                && command.DeviceInfo == deviceInfoSerialized
                && command.Language == customerOtp.Language));
        }

        [Test]
        public void SetUpOtp_InsertedCustomerOtp_ReturnResult()
        {
            // Arrange
            var customerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                DeviceInfo = new DeviceInfo
                {
                    IP = "2323232"
                },
                Language = "E"
            };
            var expectedResult = new Result
            {
                Code = 3,
                Message = "Message"
            };
            otpService.VerifyOtp(customerOtp.SecretKey, customerOtp.Otp, 30, 6).Returns(true);
            dynamicRepository
                .When(repo => repo.Execute(Arg.Any<InsertCustomerOtpCommand>()))
                .Do((callInfo) =>
                {
                    var command = callInfo[0] as InsertCustomerOtpCommand;
                    command.ErrCode = expectedResult.Code;
                    command.ErrMsg = expectedResult.Message;
                });

            // Act
            var actualResult = customerOtpService.SetUpOtp(customerOtp);

            // Assert
            Assert.IsTrue(expectedResult.Code == actualResult.Code && expectedResult.Message == actualResult.Message);
        }

        #endregion Set Up Otp

        #region Get Cutomer OTP

        [TestCase(0)]
        [TestCase(-1)]
        public void GetCustomerOtp_CustIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int custId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.GetCustomerOtp(custId));
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void GetCustomerOtp_SubAccountIdCannotBeNegative_ThrowArgumentOutOfRangeException(int subAccountId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.GetCustomerOtp(1, subAccountId));
        }

        [Test]
        public void GetCustomerOtp_ReadCustomerOtp_CallGet()
        {
            // Arrange
            var custId = 1;
            var subAccountId = 3;
            dynamicRepository.Get<CustomerOtpDto>(Arg.Any<GetCustomerOtpCriteria>())
                .Returns(new CustomerOtpDto());

            // Act
            customerOtpService.GetCustomerOtp(custId, subAccountId);

            // Assert
            dynamicRepository.Received()
                .Get<CustomerOtpDto>(
                    Arg.Is<GetCustomerOtpCriteria>(criteria =>
                        criteria.CustID == custId
                        && criteria.SubAccID == subAccountId));
        }

        [Test]
        public void GetCustomerOtp_DeserializeCustomerDeviceInfo_CallDeserialize()
        {
            // Arrange
            var custId = 1;
            var subAccountId = 3;
            var customerDeviceInfoJson = "customerDeviceInfoJson";
            dynamicRepository.Get<CustomerOtpDto>(Arg.Any<GetCustomerOtpCriteria>())
                .Returns(new CustomerOtpDto
                {
                    DeviceInfo = customerDeviceInfoJson,
                    CustID = custId,
                    SubAccId = subAccountId
                });

            // Act
            customerOtpService.GetCustomerOtp(custId, subAccountId);

            // Assert
            jsonHelper.Received().Deserialize<DeviceInfo>(customerDeviceInfoJson);
        }

        [Test]
        public void GetCustomerOtp_ReturnCustomerOtp()
        {
            // Arrange
            var custId = 1;
            var subAccountId = 3;
            var secretKey = "secretKey";
            dynamicRepository.Get<CustomerOtpDto>(Arg.Any<GetCustomerOtpCriteria>())
                .Returns(new CustomerOtpDto
                {
                    SecretKey = secretKey,
                    CustID = custId,
                    SubAccId = subAccountId
                });
            var deserializedDevicedInfo = new DeviceInfo
            {
                IP = "2",
                BrowserName = "BrowserName",
                BrowserVersion = "BrowserVersion",
                DeviceName = "DeviceName",
                DeviceVersion = "DeviceVersion"
            };
            jsonHelper.Deserialize<DeviceInfo>(Arg.Any<string>()).Returns(deserializedDevicedInfo);

            // Act
            var actualCustomerOtp = customerOtpService.GetCustomerOtp(custId, subAccountId);

            // Assert
            Assert.IsTrue(actualCustomerOtp.CustId == custId
                && actualCustomerOtp.SubAccId == subAccountId
                && actualCustomerOtp.SecretKey == secretKey
                && actualCustomerOtp.DeviceInfo.IP == deserializedDevicedInfo.IP
                && actualCustomerOtp.DeviceInfo.BrowserName == deserializedDevicedInfo.BrowserName
                && actualCustomerOtp.DeviceInfo.BrowserVersion == deserializedDevicedInfo.BrowserVersion
                && actualCustomerOtp.DeviceInfo.DeviceName == deserializedDevicedInfo.DeviceName
                && actualCustomerOtp.DeviceInfo.DeviceVersion == deserializedDevicedInfo.DeviceVersion);
        }

        #endregion Get Cutomer OTP

        #region Verify Otp

        [Test]
        public void VerifyOtp_VerifyOtpParamIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => customerOtpService.VerifyOtp(null));
        }

        [Test]
        public void VerifyOtp_CallGetCustomerOtp()
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Otp = "OTP",
                DeviceInfo = new DeviceInfo { IP = "IP" }
            };
            customerOtpService
                .GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new CustomerOtp());

            // Act
            customerOtpService.VerifyOtp(actualCustomerOtp);

            // Assert
            customerOtpService.Received().GetCustomerOtp(actualCustomerOtp.CustId, actualCustomerOtp.SubAccId);
        }

        [Test]
        public void VerifyOtp_CallHelperForVerifyingOtp()
        {
            // Arrange
            var returnCustomerOtp = new CustomerOtp
            {
                SecretKey = "SecretKey"
            };
            customerOtpService
                .GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>())
                .Returns(returnCustomerOtp);
            var needVerifiedCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Otp = "OTP",
                DeviceInfo = new DeviceInfo { IP = "IP" }
            };

            // Act
            customerOtpService.VerifyOtp(needVerifiedCustomerOtp);

            // Assert
            otpService.Received().VerifyOtp(returnCustomerOtp.SecretKey, needVerifiedCustomerOtp.Otp);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyOtp_ExecuteUpdateCustomerStatusAfterVerifyOTPCommand(bool isCorrectOtp)
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Otp = "OTP",
                DeviceInfo = new DeviceInfo { IP = "IP" },
                Language = "E"
            };
            customerOtpService
                .GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(isCorrectOtp);

            // Act
            customerOtpService.VerifyOtp(actualCustomerOtp);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<UpdateCustomerStatusAfterVerifyOTPCommand>(
                    command => command.CustID == actualCustomerOtp.CustId
                    && command.SubAccID == actualCustomerOtp.SubAccId
                    && command.CheckedOTP == isCorrectOtp
                    && command.IP == actualCustomerOtp.DeviceInfo.IP
                    && command.Language == actualCustomerOtp.Language));
        }

        [Test]
        public void VerifyOtp_OtpIsNotCorrectAndAccountIsClosed_CloseCasinoAccount()
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                Otp = "OTP",
                DeviceInfo = new DeviceInfo { IP = "IP" },
                Language = "E"
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp());
            var incorrect = false;
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(incorrect);
            var closedErrorCode = 200;
            dynamicRepository
               .When(repo => repo.Execute(Arg.Any<UpdateCustomerStatusAfterVerifyOTPCommand>()))
               .Do((callInfo) =>
               {
                   var command = callInfo[0] as UpdateCustomerStatusAfterVerifyOTPCommand;
                   command.ErrCode = closedErrorCode;
               });

            // Act
            customerOtpService.VerifyOtp(actualCustomerOtp);

            // Assert
            customerOtpService.Received().CloseCasinoAccount(actualCustomerOtp.CustId, actualCustomerOtp.DeviceInfo.IP);
        }

        [Test]
        public void VerifyOtp_OtpIsInCorrect_ReturnInvalidOtpMessage()
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                Otp = "OTP",
                DeviceInfo = new DeviceInfo { IP = "IP" },
                Language = "E"
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // Act
            var actualResult = customerOtpService.VerifyOtp(actualCustomerOtp);

            // Assert
            Assert.AreEqual(actualResult.Message, Security.OtpIsInvalidOrExpired);
        }

        [Test]
        public void VerifyOtp_OtpIsCorrect_ReturnOKResult()
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                Otp = "OTP",
                DeviceInfo = new DeviceInfo { IP = "IP" },
                Language = "E"
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            // Act
            var actualResult = customerOtpService.VerifyOtp(actualCustomerOtp);

            // Assert
            Assert.IsTrue(actualResult.IsOk);
        }

        #endregion Verify Otp

        #region Remove Secret Key

        [Test]
        public void RemoveSecretKey_RemoveSecretKeyParamIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => customerOtpService.RemoveSecretKey(null));
        }

        [Test]
        public void RemoveSecretKey_CallExecuteDeleteCustOtpSecretKeyCommand()
        {
            // Arrange
            var param = new RemoveSecretKeyParam
            {
                CustId = 1,
                ActorId = 2,
                SubAccountId = 3,
                IP = "IP",
                Language = "E"
            };

            // Act
            customerOtpService.RemoveSecretKey(param);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<DeleteCustOtpSecretKeyCommand>(
                    command => command.CustID == param.CustId
                        && command.SubAccID == param.SubAccountId
                        && command.UserID == param.ActorId
                        && command.Language == param.Language
                        && command.IP == param.IP));
        }

        [Test]
        public void RemoveSecretKey_ResetDownlineOtp_CallChangeOtpStatusToDisableDownlineStatus()
        {
            // Arrange
            var param = new RemoveSecretKeyParam
            {
                CustId = 1,
                ActorId = 2,
                SubAccountId = 3,
                IP = "IP",
                Language = "E",
                IsDisableOtp = true
            };

            dynamicRepository
              .When(repo => repo.Execute(Arg.Any<DeleteCustOtpSecretKeyCommand>()))
              .Do((callInfo) =>
              {
                  var command = callInfo[0] as DeleteCustOtpSecretKeyCommand;
                  command.ErrCode = 0; // Successfully
              });

            // Act
            customerOtpService.RemoveSecretKey(param);

            // Assert
            customerOtpService.Received().ChangeOtpStatus(Arg.Is<ChangeOtpStatusParam>(
                statusParam => statusParam.CustId == param.CustId
                    && statusParam.SubAccountId == param.SubAccountId
                    && statusParam.ActorId == param.ActorId
                    && statusParam.IsEnable == false
                    && statusParam.Language == param.Language
                    && statusParam.IP == param.IP));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RemoveSecretKey_RemovedSecretKey_ReturnRemovingStatus(bool isRemoved)
        {
            // Arrange
            var param = new RemoveSecretKeyParam
            {
                CustId = 1,
                ActorId = 2,
                SubAccountId = 3,
                IP = "IP",
                Language = "E",
            };

            dynamicRepository
              .When(repo => repo.Execute(Arg.Any<DeleteCustOtpSecretKeyCommand>()))
              .Do((callInfo) =>
              {
                  var command = callInfo[0] as DeleteCustOtpSecretKeyCommand;
                  command.ErrCode = isRemoved ? 0 : 1;
              });

            // Act
            var removeStatus = customerOtpService.RemoveSecretKey(param);

            // Assert
            Assert.AreEqual(removeStatus.Code, isRemoved ? 0 : 1);
        }

        #endregion Remove Secret Key
    }
}