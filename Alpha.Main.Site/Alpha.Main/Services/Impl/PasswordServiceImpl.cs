namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using AppResources;
    using Criteria.Password;
    using DataAccesses;
    using Models;
    using Params.Password;
    using Sunplus.Agent.Model.Casino;
    using Sunplus.Agent.Utilities;

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
            var rawCurrentPassword = DecryptPassword(param.CurrentPassword, param.HashKey);
            var rawNewPassword = DecryptPassword(param.NewPassword, param.HashKey);
            var validateResult = ValidatePassword(
                false,
                rawCurrentPassword,
                rawNewPassword,
                new List<string>
                {
                    param.ActorName,
                    param.NickName
                },
                false);

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            var encodedPassword = HashPassword(rawNewPassword);
            var criteria = new UpdateMainAccountPasswordCriteria
            {
                CustId = param.CustId,
                DisCasino = param.IsCasinoDisabled,
                Language = param.LanguageKey,
                LoginIp = param.Ip,
                OldPwd = HashPassword(rawCurrentPassword),
                UserPwd = encodedPassword,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful
                && param.IsCasinoSync)
            {
                ChangeCasinoAccountPassword(param.CustId, encodedPassword, param.ActorId, param.ActorName, param.Ip);
            }

            return updateResult;
        }

        private void ChangeCasinoAccountPassword(
            int custId,
            string encodedPassword,
            int actorId,
            string actorName,
            string ip)
        {
            var isCustomerExist = casinoService.Customer.CheckCustSetting(custId);
            if (isCustomerExist)
            {
                casinoService.Customer.UpdateCustPwd(
                    custId,
                    encodedPassword,
                    ip,
                    actorId,
                    actorName,
                    Sunplus.Agent.Model.BL.Casino.CasinoBO.AgentSystem);
            }
        }

        public Result ChangeSubAccountPassword(ChangeSubAccountPasswordParam param)
        {
            var rawCurrentPassword = DecryptPassword(param.CurrentPassword, param.HashKey);
            var rawNewPassword = DecryptPassword(param.NewPassword, param.HashKey);
            var validateResult = ValidatePassword(
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

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            var criteria = new UpdateSubAccountPasswordCriteria
            {
                CustID = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.Ip,
                OldPwd = HashPassword(rawCurrentPassword),
                UserPwd = HashPassword(rawNewPassword),
                SubAccID = param.SubAccountId
            };

            return passwordDataAccess.UpdatePassword(criteria);
        }

        public Result ResetMainAccountPassword(ResetMainAccountPasswordParam param)
        {
            var rawPassword = DecryptPassword(param.NewPassword, param.HashKey);
            var validateResult = ValidatePassword(
                true,
                string.Empty,
                rawPassword,
                new List<string>
                {
                    param.ActorName
                },
                param.IsResetMemberPassword);

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            var encodedPassword = HashPassword(rawPassword);
            var criteria = new ResetMainAccountPasswordCriteria
            {
                CustID = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.Ip,
                UserPwd = encodedPassword,
                SubAccID = param.SubAccountId,
                DisCasino = param.IsCasinoDisabled,
                UserID = param.ActorId
            };

            var updateResult = passwordDataAccess.UpdatePassword(criteria);

            if (updateResult.IsSuccessful
              && param.IsCasinoSync)
            {
                ChangeCasinoAccountPassword(param.CustId, encodedPassword, param.ActorId, param.ActorName, param.Ip);
            }

            return updateResult;
        }

        public Result ResetSubAccountPassword(ResetSubAccountPasswordParam param)
        {
            var rawNewPassword = DecryptPassword(param.NewPassword, param.HashKey);
            var validateResult = ValidatePassword(
                true,
                string.Empty,
                rawNewPassword,
                new List<string>
                {
                    param.ActorName
                },
                false);

            if (!validateResult.IsOK)
            {
                return validateResult;
            }

            var criteria = new ResetSubAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.Ip,
                UserPwd = HashPassword(rawNewPassword),
                SubAccId = param.SubAccountId,
                UserID = param.ActorId
            };

            return passwordDataAccess.UpdatePassword(criteria);
        }

        private Result ValidatePassword(
            bool isResetPassword,
            string currentPassword,
            string newPassword,
            IEnumerable<string> invalidPasswords,
            bool isResetMemberPassword)
        {
            if ((!isResetPassword && string.IsNullOrWhiteSpace(currentPassword))
                || string.IsNullOrWhiteSpace(newPassword))
            {
                return new Result { Code = Result.ErrorCode, Messsage = Security.NotAuthorized };
            }

            if (!IsMatchedRules(newPassword, isResetPassword))
            {
                return new Result { Code = Result.ErrorCode, Messsage = Security.lblPasswordRequire };
            }

            if (!isResetPassword && string.Compare(newPassword, currentPassword, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new Result { Code = Result.ErrorCode, Messsage = Security.lblPasswordDOldOne }; //// will update resource later
            }

            if (string.Compare(newPassword, "59D7!4$h", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(newPassword, "493abcDE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new Result { Code = Result.ErrorCode, Messsage = Security.lblPasswordWrongRule };
            }

            foreach (var invalidPassword in invalidPasswords)
            {
                if (!string.IsNullOrWhiteSpace(invalidPassword)
                    && newPassword.ToLower().Contains(invalidPassword.ToLower()))
                {
                    return new Result { Code = Result.ErrorCode, Messsage = Security.lblPasswordDAccountName };
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

        private string HashPassword(string password)
        {
            return encryptor.CfsEnCode(password);
        }

        private string DecryptPassword(string password, string encryptKey)
        {
            return encryptor.RC4(encryptKey, encryptor.HexDecode(password));
        }
    }
}