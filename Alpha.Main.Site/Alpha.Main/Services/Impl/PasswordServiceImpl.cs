namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Criteria.Password;
    using DataAccesses;
    using Models;
    using Params.Password;
    using Sunplus.Agent.Model.Casino;
    using Sunplus.Agent.Utilities;

    public class PasswordServiceImpl : PasswordService
    {
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

        public Result ChangeMainAccountPassword(UpdateMainAccountPasswordParam param)
        {
            var encodedPassword = encryptor.CfsEnCode(param.NewPassword);
            var criteria = new UpdateMainAccountPasswordCriteria
            {
                CustId = param.CustId,
                DisCasino = param.IsDisableCasino,
                Language = param.LanguageKey,
                LoginIp = param.Ip,
                OldPwd = encryptor.CfsEnCode(param.CurrentPassword),
                UserPwd = encodedPassword,
                UserId = param.ActorId
            };

            var updateResult = passwordDataAccess.ChangeMainAccountPassword(criteria);

            if (updateResult.IsSuccessful
                && param.IsCasinoSync)
            {
                ChangeCasinoAccountPassword(
                    param.CustId,
                    encodedPassword,
                    param.ActorId,
                    param.ActorName,
                    param.Ip);
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

        public Result ChangeSubAccountPassword(UpdateSubAccountPasswordParam param)
        {
            var criteria = new UpdateSubAccountPasswordCriteria
            {
                CustID = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.Ip,
                OldPwd = encryptor.CfsEnCode(param.CurrentPassword),
                UserPwd = encryptor.CfsEnCode(param.NewPassword),
                SubAccID = param.SubAccountId
            };

            return passwordDataAccess.ChangeSubAccountPassword(criteria);
        }

        public Result ResetMainAccountPassword(ResetMainAccountPasswordParam param)
        {
            var criteria = new ResetMainAccountPasswordCriteria
            {
                CustID = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.Ip,
                UserPwd = encryptor.CfsEnCode(param.NewPassword),
                SubAccID = param.SubAccountId,
                DisCasino = param.IsDisableCasino,
                UserID = param.ActorId
            };

            return passwordDataAccess.ResetMainAccountPassword(criteria);
        }

        public Result ResetSubAccountPassword(ResetSubAccountPasswordParam param)
        {
            var criteria = new ResetSubAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.Ip,
                UserPwd = encryptor.CfsEnCode(param.NewPassword),
                SubAccId = param.SubAccountId,
                UserID = param.ActorId
            };

            return passwordDataAccess.ResetSubAccountPassword(criteria);
        }

        private Result IsNewPasswordValid(
            string currentPassword,
            string newPassword,
            IEnumerable<string> invalidPasswords)
        {
            if (string.Compare(newPassword, currentPassword, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new Result { Code = Result.ErrorCode, Messsage = "Resources.Lang.lblPasswordDOldOne" }; //// will update resource later
            }

            if (string.Compare(newPassword, "59D7!4$h", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(newPassword, "493abcDE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new Result { Code = Result.ErrorCode, Messsage = "Resources.Lang.lblPasswordCSample" };
            }

            foreach (var invalidPassword in invalidPasswords)
            {
                if (!string.IsNullOrWhiteSpace(invalidPassword)
                    && newPassword.ToLower().Contains(invalidPassword.ToLower()))
                {
                    return new Result { Code = Result.ErrorCode, Messsage = "Resources.Lang.lblPasswordDAccountName" };
                }
            }

            return new Result();
        }
    }
}