namespace Alpha.Main.Site.Handlers
{
    using System;
    using Alpha.SecurityCode.Web;
    using AppResources;
    using Helpers;
    using Main.Models;
    using Main.Models.CustomerProfiles;
    using Main.Models.Otp;
    using MediatR;
    using Params.Otp;
    using Requests.Otp;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Security;

    public class CustomerOtpHandler :
        IRequestHandler<SetUpOtpQuery, OtpViewModel>,
        IRequestHandler<ResetOtpQuery, ResetOtpViewModel>,
        IRequestHandler<ChangeOtpStatusCommand, Result>,
        IRequestHandler<RemoveOtpCommand, Result>,
        IRequestHandler<ResetOtpCommand, ResetOtpViewModel>,
        IRequestHandler<SetUpOtpCommand, Result>,
        IRequestHandler<VerifyOtpCommand, Result>,
        IRequestHandler<VerifyOtpDifferentDeviceQuery, VerifyOtpDifferenceDeviceViewModel>,
        IRequestHandler<CheckDifferenceDeviceCommand, bool>
    {
        private const string SecretKeySessionKey = "CustomerOtpHandler.SecretKeySessionKey";

        private readonly CustomerOtpService customerOtpService;
        private readonly IUserSession userSession;
        private readonly SessionManager sessionManager;
        private readonly CustomerProfileService customerProfileService;
        private readonly BrowserHelper browserHelper;

        public CustomerOtpHandler(
            CustomerOtpService customerOtpService,
            IUserSession userSession,
            SessionManager sessionManager,
            CustomerProfileService customerProfileService,
            BrowserHelper browserHelper)
        {
            this.customerOtpService = customerOtpService;
            this.userSession = userSession;
            this.sessionManager = sessionManager;
            this.customerProfileService = customerProfileService;
            this.browserHelper = browserHelper;
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
                var device = browserHelper.GetDevice();
                viewModel.Result = customerOtpService.RemoveSecretKey(new RemoveSecretKeyParam
                {
                    CustId = message.CustId,
                    ActorId = message.ActorId,
                    IP = message.IP,
                    Language = message.Language,
                    SubAccId = message.SubAccountId,
                    DeviceInfo = device.Info
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

            var profile = customerProfileService.GetProfilePicture(userSession.CustId);

            return new VerifyOtpDifferenceDeviceViewModel
            {
                Username = message.UserName,
                AlternateUsername = BuildAlternateUsername(message.IsSubAcc, message.SubUserName, message.NickName),
                UsernameShower = BuildUsernameShower(message.IsSubAcc, message.SubUserName, message.NickName, message.UserName),
                EmbedImage = GenerateImage(profile)
            };
        }

        public Result Handle(VerifyOtpCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var deviceInfo = browserHelper.GetDevice();
            var result = customerOtpService.VerifyOtp(new CustomerOtp
            {
                CustId = message.CustId,
                SubAccId = message.SubAccountId,
                Otp = message.Otp,
                Device = deviceInfo,
                Language = message.Language
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
                SubAccId = message.SubAccountId
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
                CustId = message.CustId,
                SubAccountId = message.SubAccountId,
                Username = message.Username
            };
        }

        public OtpViewModel Handle(SetUpOtpQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var viewModel = new OtpViewModel
            {
                CustomerOtp = customerOtpService.GetCustomerOtp(message.CustId, message.SubAccountId),
                IsAlreadySetup = true
            };

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

            return viewModel;
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

            return true;
        }

        private string GenerateImage(ProfilePicture profilePicture)
        {
            if (profilePicture.ImageData != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.ImageData)}";
            }

            return string.Empty;
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

        public bool Handle(CheckDifferenceDeviceCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            var enabled = customerOtpService.IsEnabledOtp(message.CustId, message.SubAccountId);

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
    }
}