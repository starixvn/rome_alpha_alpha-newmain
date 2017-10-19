namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using AppResources;
    using CasinoLibrary.Model.UserException;
    using Criteria.Password;
    using DataAccesses;
    using Fanex.Logging.Extensions.Web;
    using Models;
    using Params.Password;
    using Sunplus.Agent.Model.Casino;
    using Sunplus.Agent.Utilities;

    public class PasswordServiceImpl : PasswordService
    {
        private const int AgentSystem = 2;
        private static readonly Regex PasswordRule1 = new Regex(@"^(?=.*\d).{8,100}$");
        private static readonly Regex PasswordRule2 = new Regex(@"^(?=.*[a-z]).{8,100}$");
        private static readonly Regex PasswordRule3 = new Regex(@"^(?=.*[A-Z]).{8,100}$");
        private static readonly Regex PasswordRule4 = new Regex(@"^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$");
        private static readonly Regex MemberPasswordRule = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$");
        private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        private readonly Encryptor encryptor;
        private readonly PasswordDataAccess passwordDataAccess;
        private readonly CasinoService casinoService;

        public PasswordServiceImpl(
            Encryptor encryptor,
            PasswordDataAccess passwordDataAccess,
            CasinoService casinoService)
        {
            this.encryptor = encryptor;
            this.passwordDataAccess = passwordDataAccess;
            this.casinoService = casinoService;
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
                DisCasino = param.IsCasinoDisabled,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                OldPwd = Encode(rawCurrentPassword),
                UserPwd = encodedPassword,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful
                && param.IsCasinoSync)
            {
                ChangeCasinoAccountPassword(param.CustId, encodedPassword, param.ActorId, param.ActorName, param.IP);
            }

            return updateResult;
        }

        internal void ChangeCasinoAccountPassword(
            int custId,
            string encodedPassword,
            int actorId,
            string actorName,
            string ip)
        {
            if (casinoService.Customer != null)
            {
                try
                {
                    casinoService.Customer.UpdateCustPwd(
                        custId,
                        encodedPassword,
                        ip,
                        actorId,
                        actorName,
                        AgentSystem);
                }
                catch (ErrorException ex)
                {
                    ExceptionHandler.Handle(new Exception(ex.Message), TraceEventType.Information);
                }
            }
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

            return passwordDataAccess.UpdatePassword(criteria);
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
                DisCasino = param.IsCasinoDisabled,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful
              && param.IsCasinoSync)
            {
                ChangeCasinoAccountPassword(param.CustId, encodedPassword, param.ActorId, param.ActorName, param.IP);
            }

            return updateResult;
        }

        public Result ResetSubAccountPassword(ResetSubAccountPasswordParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "Param cannot be null");
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

            return passwordDataAccess.UpdatePassword(criteria);
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
                    && newPassword.ToLower().Contains(invalidPassword.ToLower()))
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
            return encryptor.RC4(encryptKey, encryptor.HexDecode(password));
        }
    }
}
