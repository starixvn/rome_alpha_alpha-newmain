namespace Alpha.Main.Tests.Services
{
    using Alpha.Web.Models;
    using AppResources;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria.Otp;
    using Main.Helpers;
    using Main.Models.Otp;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.OTP.Services;
    using Params.Otp;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CustomResult = Alpha.Main.Models.Result;

    [TestFixture]
    public class CustomerOtpServiceImplTests
    {
        private CompareLogic compareLogic;
        private CustomerOtpServiceImpl customerOtpService;
        private IDynamicRepository dynamicRepository;
        private OtpService otpService;
        private JsonHelper jsonHelper;
        private INetThrottlingService netThrottlingService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            otpService = Substitute.For<OtpService>();
            jsonHelper = Substitute.For<JsonHelper>();
            netThrottlingService = Substitute.For<INetThrottlingService>();

            customerOtpService = Substitute.ForPartsOf<CustomerOtpServiceImpl>(dynamicRepository, otpService, jsonHelper, netThrottlingService);
            compareLogic = new CompareLogic();
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
                SubAccId = 3,
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
                                && command.CustId == param.CustId
                                && command.SubAccId == param.SubAccId
                                && command.UserId == param.ActorId
                                && command.Enable == param.IsEnable));
        }

        [Test]
        public void ChangeOtpStatus_ExecuteUpdateCustomerOtpStatusCommand_ReturnResult()
        {
            // Arrange
            var param = new ChangeOtpStatusParam();
            var expectedResult = new CustomResult
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
                && actualCustomerOtp.QRCode == quickResponseCode
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
                criteria => criteria.CustId == custId
                        && criteria.SubAccId == subAccId));
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
                QRCode = "aaa",
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
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "2323232",
                        BrowserName = "BrowserName",
                        BrowserVersion = "BrowserVersion",
                        DeviceName = "DeviceName",
                        DeviceVersion = "DeviceVersion"
                    }
                },
                QRCode = "aaa",
                SecretKey = "4545",
                Language = "E"
            };
            otpService.VerifyOtp(customerOtp.SecretKey, customerOtp.Otp, 30, 6).Returns(true);

            // Act
            customerOtpService.SetUpOtp(customerOtp);

            // Assert
            jsonHelper.Received().Serialize(Arg.Is<DeviceInfo>(deviceInfo =>
                deviceInfo.IP == customerOtp.Device.Info.IP
                && deviceInfo.BrowserName == customerOtp.Device.Info.BrowserName
                && deviceInfo.BrowserVersion == customerOtp.Device.Info.BrowserVersion
                && deviceInfo.DeviceName == customerOtp.Device.Info.DeviceName
                && deviceInfo.DeviceVersion == customerOtp.Device.Info.DeviceVersion));
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
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "2323232",
                        BrowserName = "BrowserName",
                        BrowserVersion = "BrowserVersion",
                        DeviceName = "DeviceName",
                        DeviceVersion = "DeviceVersion"
                    }
                },
                QRCode = "aaa",
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
                command => command.CustId == customerOtp.CustId
                && command.SubAccId == customerOtp.SubAccId
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
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "2323232"
                    }
                },
                Language = "E"
            };
            var expectedResult = new CustomResult
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
                        criteria.CustId == custId
                        && criteria.SubAccId == subAccountId));
        }

        ////[Test]
        ////public void GetCustomerOtp_DeserializeCustomerDeviceInfo_CallDeserialize()
        ////{
        ////    // Arrange
        ////    var custId = 1;
        ////    var subAccountId = 3;
        ////    var customerDeviceInfoJson = "customerDeviceInfoJson";
        ////    dynamicRepository.Get<CustomerOtpDto>(Arg.Any<GetCustomerOtpCriteria>())
        ////        .Returns(new CustomerOtpDto
        ////        {
        ////            DeviceInfo = customerDeviceInfoJson,
        ////            CustID = custId,
        ////            SubAccId = subAccountId
        ////        });

        ////    // Act
        ////    customerOtpService.GetCustomerOtp(custId, subAccountId);

        ////    // Assert
        ////    jsonHelper.Received().Deserialize<Device>(customerDeviceInfoJson);
        ////}

        [Test]
        public void GetCustomerOtp_UserHaveNotSetup_ReturnDefaultCustomerOtp()
        {
            // Arrange
            var custId = 1;
            var subAccountId = 3;
            CustomerOtpDto customerOtpDto = null;
            dynamicRepository.Get<CustomerOtpDto>(Arg.Any<GetCustomerOtpCriteria>())
                .Returns(customerOtpDto);

            // Act
            var customerOtp = customerOtpService.GetCustomerOtp(custId, subAccountId);

            // Assert
            Assert.IsTrue(!customerOtp.Enabled && string.IsNullOrWhiteSpace(customerOtp.SecretKey));
        }

        [Test]
        public void GetCustomerOtp_UserRemovedOtpBefore_ReturnDefaultCustomerOtp()
        {
            // Arrange
            var custId = 1;
            var subAccountId = 3;
            dynamicRepository.Get<CustomerOtpDto>(Arg.Any<GetCustomerOtpCriteria>())
                .Returns(new CustomerOtpDto { IsDelete = true });

            // Act
            var customerOtp = customerOtpService.GetCustomerOtp(custId, subAccountId);

            // Assert
            Assert.IsTrue(!customerOtp.Enabled && string.IsNullOrWhiteSpace(customerOtp.SecretKey));
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
                    CustId = custId,
                    SubAccId = subAccountId,
                    CreatedDate = DateTime.Now
                });

            var expectedCustomerOtp = new CustomerOtp
            {
                CustId = custId,
                SubAccId = subAccountId,
                SecretKey = secretKey
            };

            // Act
            var actualCustomerOtp = customerOtpService.GetCustomerOtp(custId, subAccountId);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedCustomerOtp, actualCustomerOtp).AreEqual);
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
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "IP"
                    }
                }
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
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "IP"
                    }
                }
            };

            // Act
            customerOtpService.VerifyOtp(needVerifiedCustomerOtp);

            // Assert
            otpService.Received().VerifyOtp(returnCustomerOtp.SecretKey, needVerifiedCustomerOtp.Otp);
        }

        [Test]
        public void VerifyOtp_OtpIsCorrect_ExecuteInsertCustomerDeviceInfoCommand()
        {
            // Arrange
            var param = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Otp = "OTP",
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "IP"
                    }
                },
                Language = "E"
            };

            customerOtpService
                .GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            var deviceInfoSerialized = "deviceInfoSerialized";
            jsonHelper.Serialize(Arg.Any<object>()).Returns(deviceInfoSerialized);

            var command = new InsertCustomerDeviceInfoCommand
            {
                CustId = param.CustId,
                DeviceId = param.Device.Id,
                DeviceInfo = deviceInfoSerialized,
                IP = param.Device.Info.IP,
                Language = param.Language,
                SubAccId = param.SubAccId
            };

            // Act
            customerOtpService.VerifyOtp(param);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<InsertCustomerDeviceInfoCommand>(
                p => compareLogic.Compare(p, command).AreEqual));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyOtp_ExecuteUpdateCustomerStatusAfterVerifyOTPCommand(bool isCorrectOtp)
        {
            // Arrange
            var param = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 2,
                Otp = "OTP",
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "IP"
                    }
                },
                Language = "E"
            };

            var changeStatusCommand = new UpdateCustomerStatusAfterVerifyOtpCommand
            {
                CheckedOtp = isCorrectOtp,
                IP = param.Device.Info.IP,
                Language = param.Language,
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                ErrCode = 1,
                ErrMsg = "Data is not valid"
            };
            customerOtpService
                .GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(isCorrectOtp);

            // Act
            customerOtpService.VerifyOtp(param);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<UpdateCustomerStatusAfterVerifyOtpCommand>(
                    command => compareLogic.Compare(command, changeStatusCommand).AreEqual));
        }

        [Test]
        public async Task VerifyOtp_OtpIsInCorrect_ReturnInvalidOtpMessage()
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                Otp = "OTP",
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "IP"
                    }
                },
                Language = "E"
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // Act
            var actualResult = await customerOtpService.VerifyOtp(actualCustomerOtp);

            // Assert
            Assert.AreEqual(actualResult.Message, Security.OtpInvalidOrExpiredAndContactUpline);
        }

        [Test]
        public async Task VerifyOtp_OtpIsCorrect_ReturnOKResult()
        {
            // Arrange
            var actualCustomerOtp = new CustomerOtp
            {
                CustId = 1,
                SubAccId = 0,
                Otp = "OTP",
                Device = new Device
                {
                    Info = new DeviceInfo
                    {
                        IP = "IP"
                    }
                },
                Language = "E"
            };
            customerOtpService.GetCustomerOtp(Arg.Any<int>(), Arg.Any<int>()).Returns(new CustomerOtp());
            otpService.VerifyOtp(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            dynamicRepository
               .When(repo => repo.Execute(Arg.Any<UpdateCustomerStatusAfterVerifyOtpCommand>()))
               .Do((callInfo) =>
               {
                   var command = callInfo[0] as UpdateCustomerStatusAfterVerifyOtpCommand;
                   command.ErrCode = 0;
               });

            // Act
            var actualResult = await customerOtpService.VerifyOtp(actualCustomerOtp);

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
                SubAccId = 3,
                IP = "IP",
                Language = "E"
            };

            var deviceInfoSerialized = "deviceInfoSerialized";
            jsonHelper.Serialize(Arg.Any<object>()).Returns(deviceInfoSerialized);
            var expectedCommand = new DeleteCustOtpSecretKeyCommand
            {
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                IP = param.IP,
                Language = param.Language,
                UserId = param.ActorId,
                ErrCode = 1,
                ErrMsg = "Data is not valid",
                DeviceInfo = deviceInfoSerialized
            };

            // Act
            customerOtpService.RemoveSecretKey(param);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<DeleteCustOtpSecretKeyCommand>(
                    command => compareLogic.Compare(command, expectedCommand).AreEqual));
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
                SubAccId = 3,
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

        [TestCase(0, "Removed the registered device successfully.")]
        [TestCase(1, "Data is not valid")]
        [TestCase(100, "This user has no registered device to be removed.")]
        [TestCase(101, "Failed to remove registered device!")]
        [TestCase(600, "Failed to remove the registered device from disabled user.")]
        public void RemovedSecretKey_IsResetDownlineOrSubAccountIsTrue_ReturnsCorrectDeleteResult(int errCode, string message)
        {
            // Arrange
            var param = new RemoveSecretKeyParam
            {
                CustId = 1,
                ActorId = 2,
                SubAccId = 3,
                IP = "IP",
                Language = "E",
                IsResetDownlineOrSubAccount = true
            };

            dynamicRepository
              .When(repo => repo.Execute(Arg.Any<DeleteCustOtpSecretKeyCommand>()))
              .Do((callInfo) =>
              {
                  var command = callInfo[0] as DeleteCustOtpSecretKeyCommand;
                  command.ErrCode = errCode;
              });

            // Act
            var deletedResult = customerOtpService.RemoveSecretKey(param);

            // Assert
            Assert.IsTrue(deletedResult.Code == errCode && deletedResult.Message.Equals(message));
        }

        #endregion Remove Secret Key

        #region Is Difference Device

        [TestCase(0)]
        [TestCase(-1)]
        public void IsDifferenceDevice_CustIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int custId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.IsDifferenceDevice(custId, "2"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsDifferenceDevice_DeviceIdIsNullOrWhiteSpace_ThrowArgumentOutOfRangeException(string deviceId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerOtpService.IsDifferenceDevice(1, deviceId));
        }

        [Test]
        public void IsDifferenceDevice_UserUseNewDevice_ReturnTrue()
        {
            // Arrange
            var deviceUsed = new List<Device>
            {
                new Device { Id = "2" }
            };
            var deviceId = "3";
            dynamicRepository.Fetch<Device>(Arg.Any<GetDevicesCriteria>()).Returns(deviceUsed);

            // Act
            var isDiff = customerOtpService.IsDifferenceDevice(2, deviceId);

            // Assert
            Assert.IsTrue(isDiff);
        }

        [Test]
        public void IsDifferenceDevice_UserUseCurrentDevice_ReturnFalse()
        {
            // Arrange
            var deviceUsed = new List<Device>
            {
                new Device { Id = "3" }
            };
            var deviceId = "3";
            dynamicRepository.Fetch<Device>(Arg.Any<GetDevicesCriteria>()).Returns(deviceUsed);

            // Act
            var isDiff = customerOtpService.IsDifferenceDevice(2, deviceId);

            // Assert
            Assert.IsFalse(isDiff);
        }

        #endregion
    }
}
