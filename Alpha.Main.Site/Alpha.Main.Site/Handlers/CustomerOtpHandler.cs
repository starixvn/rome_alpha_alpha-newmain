namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using Alpha.SecurityCode.Web;
    using AppResources;
    using Main.Models;
    using Main.Models.Otp;
    using MediatR;
    using Params.Otp;
    using Requests.Otp;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using TelegramBot.Client.Services;
    using ViewModels.Security;
    using Web.Helpers;

    public class CustomerOtpHandler :
        IAsyncRequestHandler<SetUpOtpQuery, OtpViewModel>,
        IRequestHandler<ResetOtpQuery, ResetOtpViewModel>,
        IRequestHandler<ChangeOtpStatusCommand, Result>,
        IRequestHandler<RemoveOtpCommand, Result>,
        IRequestHandler<ResetOtpCommand, ResetOtpViewModel>,
        IRequestHandler<SetUpOtpCommand, Result>,
        IAsyncRequestHandler<VerifyOtpCommand, Result>,
        IRequestHandler<VerifyOtpDifferentDeviceQuery, VerifyOtpDifferenceDeviceViewModel>,
        IAsyncRequestHandler<CheckDifferenceDeviceCommand, bool>,
        IRequestHandler<VerifyOtpQuery, VerifyOTPViewModel>,
        IAsyncRequestHandler<GenerateOTPCommand, long>
    {
        private const string SecretKeySessionKey = "CustomerOtpHandler.SecretKeySessionKey";

        private readonly CustomerOtpService customerOtpService;
        private readonly IUserSession userSession;
        private readonly SessionManager sessionManager;
        private readonly BrowserHelper browserHelper;
        private readonly IUser user;
        private readonly NicknameChecker nicknameChecker;
        private readonly ISettingService telegramSettingService;

        public CustomerOtpHandler(
            CustomerOtpService customerOtpService,
            IUserSession userSession,
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            IUser user,
            NicknameChecker nicknameChecker,
            ISettingService telegramSettingService)
        {
            this.customerOtpService = customerOtpService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.user = user;
            this.nicknameChecker = nicknameChecker;
            this.telegramSettingService = telegramSettingService;
        }

        public Result Handle(ChangeOtpStatusCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = customerOtpService.ChangeOtpStatus(new ChangeOtpStatusParam
            {
                CustId = message.CustId,
                ActorId = message.CustId,
                IP = message.IP,
                Language = message.Language,
                IsEnable = message.IsEnable,
                SubAccId = message.SubAccountId
            });

            if (result.IsSuccessful)
            {
                SetOtpStatusToSession(message.IsEnable);
            }

            return result;
        }

        public ResetOtpViewModel Handle(ResetOtpCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var viewModel = new ResetOtpViewModel
            {
                CustId = message.CustId,
                SubAccountId = message.SubAccountId,
                Username = message.Username
            };

            if (!HasUpdatePermission(true, message.SubAccountId > 0))
            {
                viewModel.Result = Result.CreateErrorResult(Security.NotAuthorized);
            }
            else
            {
                var deviceInfo = browserHelper.GetDevice().Info;
                viewModel.Result = customerOtpService.RemoveSecretKey(new RemoveSecretKeyParam
                {
                    CustId = message.CustId,
                    ActorId = message.ActorId,
                    IP = deviceInfo.IP,
                    Language = message.Language,
                    SubAccId = message.SubAccountId,
                    DeviceInfo = deviceInfo,
                    IsResetDownlineOrSubAccount = true
                });
            }

            return viewModel;
        }

        public Result Handle(SetUpOtpCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var customerOtp = new CustomerOtp
            {
                CustId = message.CustId,
                Otp = message.Otp,
                Language = message.Language,
                SubAccId = message.SubAccountId,
                SecretKey = sessionManager.Get<string>(SecretKeySessionKey),
                Device = browserHelper.GetDevice()
            };

            var result = customerOtpService.SetUpOtp(customerOtp);

            if (result.IsSuccessful)
            {
                SetVerifiedOtp();
            }

            return result;
        }

        public VerifyOtpDifferenceDeviceViewModel Handle(VerifyOtpDifferentDeviceQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return new VerifyOtpDifferenceDeviceViewModel
            {
                Username = message.UserName,
                AlternateUsername = BuildAlternateUsername(message.IsSubAcc, message.SubUserName, message.NickName),
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.UserName),
                IsShowGenerateOTPButton = user.IsEnabledTelegramOTP,
                OTPExpirationTime = user.TelegramOTPExpirationTime
            };
        }

        public async Task<Result> Handle(VerifyOtpCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var deviceInfo = browserHelper.GetDevice();
            var result = await customerOtpService.VerifyOtp(new CustomerOtp
            {
                CustId = message.CustId,
                SubAccId = message.SubAccountId,
                Otp = message.Otp,
                Device = deviceInfo,
                Language = message.Language,
                IsEnabledTelegramOTP = user.IsEnabledTelegramOTP
            });

            if (result.IsSuccessful)
            {
                SetVerifiedOtp();
            }

            return result;
        }

        public Result Handle(RemoveOtpCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            if (!HasUpdatePermission())
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            var result = customerOtpService.RemoveSecretKey(new RemoveSecretKeyParam
            {
                CustId = message.CustId,
                ActorId = message.CustId,
                IP = message.IP,
                Language = message.Language,
                SubAccId = message.SubAccountId,
                DeviceInfo = browserHelper.GetDevice().Info
            });

            return result;
        }

        public ResetOtpViewModel Handle(ResetOtpQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return new ResetOtpViewModel
            {
                CustId = message.SubAccountId > 0 ? userSession.CustId : message.CustId,
                SubAccountId = message.SubAccountId,
                Username = message.Username
            };
        }

        public VerifyOTPViewModel Handle(VerifyOtpQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return new VerifyOTPViewModel
            {
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.UserName),
                IsShowGenerateOTPButton = user.IsEnabledTelegramOTP,
                OTPExpirationTime = user.TelegramOTPExpirationTime
            };
        }

        public async Task<OtpViewModel> Handle(SetUpOtpQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var customerOtp = customerOtpService.GetCustomerOtp(message.CustId, message.SubAccountId);

            var viewModel = new OtpViewModel
            {
                CustomerOtp = customerOtp,
                IsAlreadySetup = true,
                IsShowNicknameLink = !nicknameChecker.IsOfferForceLoginAndOverDueDate,
                IsOfferProtectedMode = message.IsOfferProtectedMode
            };

            var custBotMessenger = await telegramSettingService.GetCustBotMessengerAsync(
                message.CustId,
                message.SubAccountId,
                Language.MapNewChineseLanguageCode(message.Language));

            viewModel.RegisterAccountViewModel =
                new RegisterAccountViewModel(new ChatBotViewModel(custBotMessenger), new SetUpChatBotQuery())
                { IsOtpPage = true };

            if (!viewModel.CustomerOtp.Enabled
                && string.IsNullOrWhiteSpace(viewModel.CustomerOtp.SecretKey))
            {
                if (string.IsNullOrWhiteSpace(message.Username))
                {
                    message.Username = userSession.IsSubAcc ? userSession.SubUserName : userSession.UserName;
                }

                viewModel.CustomerOtp = customerOtpService.GenerateCustomerOtp(message.CustId, message.Username, message.SubAccountId);
                viewModel.IsAlreadySetup = false;
                sessionManager.Add(SecretKeySessionKey, viewModel.CustomerOtp.SecretKey);
            }

            return await Task.FromResult(viewModel);
        }

        private bool HasUpdatePermission(
            bool isResetOtp = false,
            bool isResetSubAccount = false)
        {
            if (userSession.IsInternal && !isResetSubAccount)
            {
                return false;
            }

            if (userSession.IsLimitedHostPermisionOnSubAccountFunction)
            {
                return false;
            }

            if (isResetOtp
                && (userSession.IsSubAcc && !userSession.SubPageAccess.UpdateAccount))
            {
                return false;
            }

            if (isResetSubAccount && !user.IsAdminCanUpdateSubAccount)
            {
                return false;
            }

            return true;
        }

        private string BuildUsernameShower(bool isSubAcc, string subAccName, string nickName, string username)
        {
            if (isSubAcc)
            {
                return subAccName;
            }

            return string.IsNullOrWhiteSpace(nickName) ? username : nickName;
        }

        private string BuildAlternateUsername(bool isSubAcc, string subAccName, string nickName)
        {
            if (isSubAcc)
            {
                return $"({Resources.subAcc.ToUpper()} {subAccName})";
            }

            return string.IsNullOrWhiteSpace(nickName) ? string.Empty : $"({nickName})";
        }

        public async Task<bool> Handle(CheckDifferenceDeviceCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            user.IsEnabledTelegramOTP = await telegramSettingService.IsEnableOTP(message.CustId, message.SubAccountId);

            var enabled = customerOtpService.IsEnabledOtp(message.CustId, message.SubAccountId) || user.IsEnabledTelegramOTP;

            SetOtpStatusToSession(enabled);

            if (!enabled)
            {
                return false;
            }

            var deviceId = browserHelper.GetDeviceId();

            return customerOtpService.IsDifferenceDevice(message.CustId, deviceId, message.SubAccountId);
        }

        private void SetOtpStatusToSession(bool isEnabledOtp)
        {
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp = isEnabledOtp;
        }

        private void SetVerifiedOtp()
        {
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        public async Task<long> Handle(GenerateOTPCommand message)
        {
            await customerOtpService.GenerateTelegramOTP(message.CustId, message.SubAccId);
            user.TelegramOTPExpirationTime = DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeMilliseconds();
            return user.TelegramOTPExpirationTime;
        }
    }
}