namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AppResources;
    using Criteria.Password;
    using DataAccesses;
    using Fanex.Cryptography.Aes;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using Models;
    using Params;
    using Params.Password;
    using Sunplus.Agent.Utilities;
    using TelegramBot.Client.Services;

    public class PasswordServiceImpl : PasswordService
    {
        private static readonly Regex PasswordRule1 = new Regex(@"^(?=.*\d).{8,100}$");
        private static readonly Regex PasswordRule2 = new Regex(@"^(?=.*[a-z]).{8,100}$");
        private static readonly Regex PasswordRule3 = new Regex(@"^(?=.*[A-Z]).{8,100}$");
        private static readonly Regex PasswordRule4 = new Regex(@"^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$");
        private static readonly Regex MemberPasswordRule = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$");
        private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        private readonly Encryptor encryptor;
        private readonly PasswordDataAccess passwordDataAccess;
        private readonly IAesService aes;
        private readonly KickoutCustomerService kickoutCustomerService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly INetThrottlingService netThrottlingService;
        private readonly INotificationService notificationService;

        public PasswordServiceImpl(
            Encryptor encryptor,
            PasswordDataAccess passwordDataAccess,
            IAesService aes,
            KickoutCustomerService kickoutCustomerService,
            IDynamicRepository dynamicRepository,
            INetThrottlingService netThrottlingService,
            INotificationService notificationService)
        {
            this.encryptor = encryptor;
            this.passwordDataAccess = passwordDataAccess;
            this.aes = aes;
            this.kickoutCustomerService = kickoutCustomerService;
            this.dynamicRepository = dynamicRepository;
            this.netThrottlingService = netThrottlingService;
            this.notificationService = notificationService;
        }

        public Result ChangeMainAccountPassword(ChangeMainAccountPasswordParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Param cannot be null");
            }

            var rawCurrentPassword = Decrypt(param.CurrentPassword, param.HashKey);
            var rawNewPassword = Decrypt(param.NewPassword, param.HashKey);
            var validateResult = Validate(
                false,
                rawCurrentPassword,
                rawNewPassword,
                new List<string>
                {
                    param.ActorName,
                    param.NickName
                },
                false);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var encodedPassword = Encode(rawNewPassword);
            var criteria = new UpdateMainAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                OldPwd = Encode(rawCurrentPassword),
                UserPwd = encodedPassword,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful && param.IsIndirectLoggedIn)
            {
                UpdateLastChangePasswordIndirectAccount(new UpdateLastChangePasswordIndirectAccountParam
                {
                    CustId = param.IndirectCustId,
                    CustIdIndirect = param.CustId,
                    DownlineUsername = param.ActorName,
                    IP = param.IP,
                    Language = param.LanguageKey,
                    SubAccId = param.IndirectSubAccId,
                    SubAccIdIndirect = param.SubAccountId,
                    UplineUsername = param.IndirectUsername
                });
            }

            return updateResult;
        }

        public Result ChangeSubAccountPassword(ChangeSubAccountPasswordParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Param cannot be null");
            }

            var rawCurrentPassword = Decrypt(param.CurrentPassword, param.HashKey);
            var rawNewPassword = Decrypt(param.NewPassword, param.HashKey);
            var validateResult = Validate(
                false,
                rawCurrentPassword,
                rawNewPassword,
                new List<string>
                {
                    param.SubUsername,
                    param.Nickname,
                    param.Username
                },
                false);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var criteria = new UpdateSubAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                OldPwd = Encode(rawCurrentPassword),
                UserPwd = Encode(rawNewPassword),
                SubAccId = param.SubAccountId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);
            if (updateResult.IsSuccessful && param.IsIndirectLoggedIn)
            {
                UpdateLastChangePasswordIndirectAccount(new UpdateLastChangePasswordIndirectAccountParam
                {
                    CustId = param.IndirectCustId,
                    CustIdIndirect = param.CustId,
                    DownlineUsername = param.ActorName,
                    IP = param.IP,
                    Language = param.LanguageKey,
                    SubAccId = param.IndirectSubAccId,
                    SubAccIdIndirect = param.SubAccountId,
                    UplineUsername = param.IndirectUsername
                });
            }

            return updateResult;
        }

        public Result ResetMainAccountPassword(ResetMainAccountPasswordParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Param cannot be null");
            }

            var rawPassword = Decrypt(param.NewPassword, param.HashKey);
            var validateResult = Validate(
                true,
                string.Empty,
                rawPassword,
                new List<string>
                {
                    param.ActorName
                },
                param.IsResetMemberPassword);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var encodedPassword = Encode(rawPassword);
            var criteria = new ResetMainAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                UserPwd = encodedPassword,
                SubAccId = param.SubAccountId,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful)
            {
                if (param.IsCasinoSync)
                {
                    ChangeCasinoAccountPassword(param.CustId, encodedPassword, param.ActorId, param.ActorName, param.IP);
                }

                Task.Run(() =>
                {
                    KickoutCustomer(new KickoutCustomerParam(
                        param.CustId,
                        param.CustLevel,
                        param.ServerId,
                        param.IP));

                    if (!param.IsResetMemberPassword)
                    {
                        ResetPasswordAlert(param.CustId, param.IP);
                    }
                });
            }

            return updateResult;
        }

        public Result SkipForceChangePassword(SkipForceChangePasswordParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Param cannot be null");
            }

            var command = new SkipForceChangePasswordCommand
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                SubAccId = param.SubAccountId
            };

            dynamicRepository.Execute(command);

            return new Result
            {
                Code = command.ErrCode,
                Message = command.ErrMsg
            };
        }

        public Result ResetSubAccountPassword(ResetSubAccountPasswordParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Param cannot be null");
            }

            if (netThrottlingService.IsBlacklist())
            {
                return Result.CreateErrorResult("Too many requests");
            }

            var rawNewPassword = Decrypt(param.NewPassword, param.HashKey);
            var validateResult = Validate(
                true,
                string.Empty,
                rawNewPassword,
                new List<string>
                {
                    param.ActorName
                },
                false);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var criteria = new ResetSubAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                UserPwd = Encode(rawNewPassword),
                SubAccId = param.SubAccountId,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful)
            {
                netThrottlingService.Throttle();

                Task.Run(() =>
                {
                    KickoutCustomer(new KickoutCustomerParam(
                        param.CustId,
                        param.CustLevel,
                        param.ServerId,
                        param.IP,
                        param.SubAccountId));

                    ResetPasswordAlert(param.CustId, param.IP, param.SubAccountId);
                });
            }

            return updateResult;
        }

        internal void KickoutCustomer(KickoutCustomerParam param)
        {
            try
            {
                if (!param.IsDisabledRecheckin)
                {
                    kickoutCustomerService.Kickout(param);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        internal void ResetPasswordAlert(int custId, string ip, int subAccId = 0)
        {
            try
            {
                notificationService.NotifyChangePassword(custId, subAccId, ip).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private Result Validate(
            bool isResetPassword,
            string currentPassword,
            string newPassword,
            IEnumerable<string> invalidPasswords,
            bool isResetMemberPassword)
        {
            if ((!isResetPassword && string.IsNullOrWhiteSpace(currentPassword))
                || string.IsNullOrWhiteSpace(newPassword))
            {
                return Result.CreateErrorResult(Security.NotAuthorized);
            }

            if (!IsMatchedRules(newPassword, isResetMemberPassword))
            {
                return Result.CreateErrorResult(Security.lblPasswordRequire);
            }

            if (!isResetPassword && string.Compare(newPassword, currentPassword, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Result.CreateErrorResult(Security.lblPasswordDOldOne); //// will update resource later
            }

            if (string.Compare(newPassword, "59D7!4$h", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(newPassword, "493abcDE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Result.CreateErrorResult(Security.lblPasswordWrongRule);
            }

            foreach (var invalidPassword in invalidPasswords)
            {
                if (!string.IsNullOrWhiteSpace(invalidPassword)
                    && newPassword.IndexOf(invalidPassword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return Result.CreateErrorResult(Security.lblPasswordDAccountName);
                }
            }

            return new Result();
        }

        /// <summary>
        /// Password must be at least 8 characters without white space and contain at least 3 of the following:
        ///  • Uppercase letter [A-Z]
        ///  • Lowercase leader [a-z]
        ///  • Numeric [0-9]
        ///  • Special character (!,@,#,etc..)
        /// For example: 59D7!4$h
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsMatchedRules(string password, bool isResetMemberPassword)
        {
            if (isResetMemberPassword)
            {
                return MemberPasswordRule.Match(password).Success && NoWhiteSpace.Match(password).Success;
            }

            var r1 = PasswordRule1.Match(password).Success ? 1 : 0;
            var r2 = PasswordRule2.Match(password).Success ? 1 : 0;
            var r3 = PasswordRule3.Match(password).Success ? 1 : 0;
            var r4 = PasswordRule4.Match(password).Success ? 1 : 0;

            return (r1 + r2 + r3 + r4 >= 3) && NoWhiteSpace.Match(password).Success;
        }

        private string Encode(string password)
        {
            return encryptor.CfsEnCode(password);
        }

        private string Decrypt(string password, string encryptKey)
        {
            return aes.Decrypt(password, encryptKey);
        }

        public void UpdateLastChangePasswordIndirectAccount(UpdateLastChangePasswordIndirectAccountParam param)
        {
            var criteria = new UpdateLastChangePasswordIndirectAccountCriteria
            {
                CustId = param.CustId,
                CustIdIndirect = param.CustIdIndirect,
                DownlineUsername = param.DownlineUsername,
                IP = param.IP,
                Language = param.Language,
                SubAccId = param.SubAccId,
                SubAccIdIndirect = param.SubAccIdIndirect,
                UplineUsername = param.UplineUsername
            };

            dynamicRepository.Execute(criteria);
        }
    }
}