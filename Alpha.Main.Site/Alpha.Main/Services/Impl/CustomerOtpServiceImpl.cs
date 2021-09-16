namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Alpha.Web.Models;
    using AppResources;
    using Criteria.Otp;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using Helpers;
    using Models.Otp;
    using ONEbook.OTP.Services;
    using Params.Otp;
    using CustomResult = Alpha.Main.Models.Result;

    public class CustomerOtpServiceImpl : CustomerOtpService
    {
        private const int TimePeriod = 30;
        private const int OtpSize = 6;
        private const int SecretKeyLength = 20;
        private const int DisabledAccountErrorCode = 200;
        private const string Issuer = "MyAppName";
        private const string NotDetectedDevice = "N/A";
        private const int InvalidDataErrorCode = 1;
        private const string InvalidDataMessage = "Data is not valid";
        private const int DownlineIsDisabledErrorCode = 600;
        private const int DownlineDoesNotHaveRegisteredDeviceErrorCode = 100;
        private const int FailedToRemoveDownlineOtpErrorCode = 101;

        private readonly IDynamicRepository dynamicRepository;
        private readonly OtpService otpService;
        private readonly JsonHelper jsonHelper;
        private readonly INetThrottlingService netThrottlingService;

        public CustomerOtpServiceImpl(
            IDynamicRepository dynamicRepository,
            OtpService otpService,
            JsonHelper jsonHelper,
            INetThrottlingService netThrottlingService)
        {
            this.dynamicRepository = dynamicRepository;
            this.otpService = otpService;
            this.jsonHelper = jsonHelper;
            this.netThrottlingService = netThrottlingService;
        }

        public virtual CustomResult ChangeOtpStatus(ChangeOtpStatusParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Change Otp Status Parameters cannot be null");
            }

            if (netThrottlingService.IsBlacklist())
            {
                return CustomResult.CreateErrorResult("Too many requests");
            }

            var command = new UpdateCustomerOtpStatusCommand
            {
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                Enable = param.IsEnable,
                IP = param.IP,
                Language = param.Language,
                UserId = param.ActorId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage
            };

            dynamicRepository.Execute(command);

            if (command.ErrCode == 0)
            {
                netThrottlingService.Throttle();
            }

            return new CustomResult
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.UpdateOtpStatusSuccess : command.ErrMsg
            };
        }

        public CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccId = 0)
        {
            if (custId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Cust Id cannot be negative or zero");
            }

            if (subAccId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Sub account Id cannot be negative");
            }

            var customerOtp = new CustomerOtp
            {
                SecretKey = otpService.GenerateSecretKey(SecretKeyLength),
                CustId = custId,
                SubAccId = subAccId
            };

            customerOtp.OtpUsername = username;
            customerOtp.QRCode = otpService.GenerateQrCode(customerOtp.SecretKey, TimePeriod, OtpSize, Issuer, customerOtp.OtpUsername);

            return customerOtp;
        }

        public bool IsEnabledOtp(int custId, int subAccId = 0)
        {
            if (custId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Cust Id cannot be negative or zero");
            }

            if (subAccId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Sub account Id cannot be negative");
            }

            var command = new CheckEnableOtpCommand
            {
                CustId = custId,
                SubAccId = subAccId
            };

            dynamicRepository.Execute(command);

            return command.Enable;
        }

        public CustomResult SetUpOtp(CustomerOtp param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Set Up Otp Parameters cannot be null");
            }

            var isCorrect = otpService.VerifyOtp(param.SecretKey, param.Otp, TimePeriod, OtpSize);

            if (!isCorrect)
            {
                return CustomResult.CreateErrorResult(Security.OtpIsInvalidOrExpired);
            }

            var deviceInfoSerialized = jsonHelper.Serialize(param.Device.Info);
            var command = new InsertCustomerOtpCommand
            {
                CustId = param.CustId,
                DeviceId = param.Device.Id,
                DeviceInfo = deviceInfoSerialized,
                IP = param.Device.Info.IP,
                Language = param.Language,
                SecretKey = param.SecretKey,
                SubAccId = param.SubAccId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage
            };

            dynamicRepository.Execute(command);

            return new CustomResult
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.AuthenticatorAppVerified : command.ErrMsg
            };
        }

        public CustomResult VerifyOtp(CustomerOtp param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Verify Otp Param Parameters cannot be null");
            }

            var customerOtp = GetCustomerOtp(param.CustId, param.SubAccId);

            var isCorrect = otpService.VerifyOtp(customerOtp.SecretKey, param.Otp, TimePeriod, OtpSize);

            if (isCorrect)
            {
                UpdateDevice(param);
            }

            var changeStatusCommand = new UpdateCustomerStatusAfterVerifyOtpCommand
            {
                CheckedOtp = isCorrect,
                IP = param.Device.Info.IP,
                Language = param.Language,
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage
            };

            dynamicRepository.Execute(changeStatusCommand);

            var result = new CustomResult
            {
                Code = changeStatusCommand.ErrCode,
                Message = isCorrect ? changeStatusCommand.ErrMsg : Security.OtpInvalidOrExpiredAndContactUpline
            };

            if (!isCorrect
                && result.Code == DisabledAccountErrorCode)
            {
                result.Message = Security.LockAccountOtpMessage;
            }

            return result;
        }

        public virtual CustomerOtp GetCustomerOtp(int custId, int subAccId = 0)
        {
            if (custId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Cust Id cannot be negative or zero");
            }

            if (subAccId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Sub account Id cannot be negative");
            }

            var customerOtpDto = dynamicRepository.Get<CustomerOtpDto>(
                new GetCustomerOtpCriteria
                {
                    CustId = custId,
                    SubAccId = subAccId
                });

            if (customerOtpDto == null
                || customerOtpDto.IsDelete)
            {
                return new CustomerOtp();
            }

            return new CustomerOtp
            {
                CustId = customerOtpDto.CustId,
                Enabled = customerOtpDto.Enabled,
                SecretKey = customerOtpDto.SecretKey,
                SubAccId = customerOtpDto.SubAccId
            };
        }

        public CustomResult RemoveSecretKey(RemoveSecretKeyParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Remove Secret Key Parameters cannot be null");
            }

            var command = new DeleteCustOtpSecretKeyCommand
            {
                CustId = param.CustId,
                SubAccId = param.SubAccId,
                IP = param.IP,
                Language = param.Language,
                UserId = param.ActorId,
                ErrCode = InvalidDataErrorCode,
                ErrMsg = InvalidDataMessage,
                DeviceInfo = jsonHelper.Serialize(param.DeviceInfo)
            };

            dynamicRepository.Execute(command);

            var deleteResult = new CustomResult
            {
                Code = command.ErrCode,
                Message = GetRemovedOtpMessage(command.ErrCode, param.IsResetDownlineOrSubAccount, command.ErrMsg)
            };

            return deleteResult;
        }

        private string GetRemovedOtpMessage(int code, bool isResetDownlineOrSubAccount, string errorMessage)
        {
            if (isResetDownlineOrSubAccount)
            {
                switch (code)
                {
                    case DownlineIsDisabledErrorCode:
                        return Security.RemoveDisableOtpDownline;

                    case DownlineDoesNotHaveRegisteredDeviceErrorCode:
                        return Security.NoRegisterDeviceError;

                    case FailedToRemoveDownlineOtpErrorCode:
                        return Security.FailedToRemoveOtp;

                    case 0:
                        return Security.RemoveRegisteredDeviceSuccess;

                    default:
                        return errorMessage;
                }
            }

            return code == 0 ? Security.RemoveOtpSuccessMessage : errorMessage;
        }

        private void UpdateDevice(CustomerOtp customerOtp)
        {
            var command = new InsertCustomerDeviceInfoCommand
            {
                CustId = customerOtp.CustId,
                DeviceId = customerOtp.Device.Id,
                DeviceInfo = jsonHelper.Serialize(customerOtp.Device.Info),
                IP = customerOtp.Device.Info.IP,
                Language = customerOtp.Language,
                SubAccId = customerOtp.SubAccId
            };

            dynamicRepository.Execute(command);
        }

        public bool IsDifferenceDevice(int custId, string deviceId, int subAccId = 0)
        {
            if (custId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Cust Id cannot be negative or zero");
            }

            if (string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentOutOfRangeException(nameof(deviceId), "Device Id cannot be negative");
            }

            if (deviceId == NotDetectedDevice)
            {
                ExceptionHandler.Handle(new NotSupportedException("User device was not detected"), TraceEventType.Information);
                return false;
            }

            var devices = dynamicRepository.Fetch<Device>(
                new GetDevicesCriteria
                {
                    CustId = custId,
                    SubAccId = subAccId
                });

            var currentDevice = devices.FirstOrDefault(device => device.Id == deviceId);

            return currentDevice == null;
        }
    }
}