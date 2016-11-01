namespace Alpha.Main.Services.Impl
{
    using System;
    using AppResources;
    using Criteria.Otp;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Models.Otp;
    using ONEbook.OTP.Services;
    using Params.Otp;
    using Sunplus.Agent.Model.Casino;

    public class CustomerOtpServiceImpl : CustomerOtpService
    {
        private const int TimePeriod = 30;
        private const int OtpSize = 6;
        private const int SecretKeyLength = 20;
        private const string Issuer = "MyAppName";
        private const string DateFormat = "yyyyMMdd_HHmm";

        private readonly IDynamicRepository dynamicRepository;
        private readonly OtpService otpService;
        private readonly JsonHelper jsonHelper;
        private readonly CasinoService casinoService;
        private readonly IDateTime dateTime;

        public CustomerOtpServiceImpl(
            IDynamicRepository dynamicRepository,
            OtpService otpService,
            JsonHelper jsonHelper,
            CasinoService casinoService,
            IDateTime dateTime)
        {
            this.dynamicRepository = dynamicRepository;
            this.otpService = otpService;
            this.jsonHelper = jsonHelper;
            this.casinoService = casinoService;
            this.dateTime = dateTime;
        }

        public virtual Result ChangeOtpStatus(ChangeOtpStatusParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Change Otp Status Parameters cannot be null");
            }

            var command = new UpdateCustomerOtpStatusCommand
            {
                CustID = param.CustId,
                SubAccID = param.SubAccountId,
                Enable = param.IsEnable,
                IP = param.IP,
                Language = param.Language,
                UserID = param.ActorId
            };

            dynamicRepository.Execute(command);

            return new Result
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.UpdateOtpStatusSuccess : command.ErrMsg
            };
        }

        public CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccountId = 0)
        {
            if (custId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Cust Id cannot be negative or zero");
            }

            if (subAccountId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Sub account Id cannot be negative");
            }

            var customerOtp = new CustomerOtp
            {
                SecretKey = otpService.GenerateSecretKey(SecretKeyLength),
                CustId = custId,
                SubAccId = subAccountId
            };

            customerOtp.OtpUsername = username;
            customerOtp.QrCode = otpService.GenerateQrCode(customerOtp.SecretKey, TimePeriod, OtpSize, Issuer, customerOtp.OtpUsername);

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
                CustID = custId,
                SubAccID = subAccId
            };

            dynamicRepository.Execute(command);

            return command.Enable;
        }

        public Result SetUpOtp(CustomerOtp param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Set Up Otp Parameters cannot be null");
            }

            var isCorrect = otpService.VerifyOtp(param.SecretKey, param.Otp, TimePeriod, OtpSize);

            if (!isCorrect)
            {
                return Result.CreateErrorResult(Security.OtpIsInvalidOrExpired);
            }

            var deviceInfoSerialized = jsonHelper.Serialize(param.DeviceInfo);
            var command = new InsertCustomerOtpCommand
            {
                CustID = param.CustId,
                DeviceInfo = deviceInfoSerialized,
                IP = param.DeviceInfo.IP,
                Language = param.Language,
                SecretKey = param.SecretKey,
                SubAccID = param.SubAccId
            };

            dynamicRepository.Execute(command);

            return new Result
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.AuthenticatorAppVerified : command.ErrMsg
            };
        }

        public Result VerifyOtp(CustomerOtp param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Verify Otp Param Parameters cannot be null");
            }

            var customerOtp = GetCustomerOtp(param.CustId, param.SubAccId);

            var isCorrect = otpService.VerifyOtp(customerOtp.SecretKey, param.Otp, TimePeriod, OtpSize);

            if (isCorrect)
            {
                UpdateDeviceInfo(param);
            }

            var changeStatusCommand = new UpdateCustomerStatusAfterVerifyOTPCommand
            {
                CheckedOTP = isCorrect,
                IP = param.DeviceInfo.IP,
                Language = param.Language,
                CustID = param.CustId,
                SubAccID = param.SubAccId
            };

            dynamicRepository.Execute(changeStatusCommand);

            var result = new Result
            {
                Code = changeStatusCommand.ErrCode,
                Message = isCorrect ? changeStatusCommand.ErrMsg : Security.OtpIsInvalidOrExpired
            };

            if (!isCorrect
                && result.Code == 200
                && param.SubAccId == 0)
            {
                CloseCasinoAccount(param.CustId, param.DeviceInfo.IP);
            }

            return result;
        }

        internal virtual void CloseCasinoAccount(int custId, string ip)
        {
            if (casinoService.Customer != null)
            {
                casinoService.Customer.UpdateCustClosedFailSecurityCode(custId, true, ip);
            }
        }

        public virtual CustomerOtp GetCustomerOtp(int custId, int subAccountId = 0)
        {
            if (custId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Cust Id cannot be negative or zero");
            }

            if (subAccountId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(custId), "Sub account Id cannot be negative");
            }

            var customerOtpDto = dynamicRepository.Get<CustomerOtpDto>(
                new GetCustomerOtpCriteria
                {
                    CustID = custId,
                    SubAccID = subAccountId
                });

            if (customerOtpDto == null)
            {
                return new CustomerOtp();
            }

            return new CustomerOtp
            {
                CustId = customerOtpDto.CustID,
                Enabled = customerOtpDto.Enabled,
                DeviceInfo = jsonHelper.Deserialize<DeviceInfo>(customerOtpDto.DeviceInfo),
                SecretKey = customerOtpDto.SecretKey,
                SubAccId = customerOtpDto.SubAccId
            };
        }

        public Result RemoveSecretKey(RemoveSecretKeyParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Remove Secret Key Parameters cannot be null");
            }

            var command = new DeleteCustOtpSecretKeyCommand
            {
                CustID = param.CustId,
                SubAccID = param.SubAccountId,
                IP = param.IP,
                Language = param.Language,
                UserID = param.ActorId
            };

            dynamicRepository.Execute(command);

            var deleteResult = new Result
            {
                Code = command.ErrCode,
                Message = command.ErrCode == 0 ? Security.RemoveOtpSuccessMessage : command.ErrMsg
            };

            return deleteResult;
        }

        private void UpdateDeviceInfo(CustomerOtp customerOtp)
        {
            var command = new UpdateCustomerDeviceInfoCommand
            {
                CustID = customerOtp.CustId,
                DeviceInfo = jsonHelper.Serialize(customerOtp.DeviceInfo),
                IP = customerOtp.DeviceInfo.IP,
                Language = customerOtp.Language,
                SubAccID = customerOtp.SubAccId
            };

            dynamicRepository.Execute(command);
        }
    }
}