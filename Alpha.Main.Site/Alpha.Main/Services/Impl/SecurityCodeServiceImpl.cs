namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Services;
    using AppResources;
    using Configurations;
    using DataAccesses;
    using Fanex.Security;
    using Models.SecurityCode;
    using SecurityCode.Configurations;
    using SecurityCode.Entities;
    using SecurityCode.Sessions;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SecurityCodeServiceImpl : SecurityCodeService
    {
        private const string MD5 = "MD5";
        private const string SHA1 = "SHA1";
        private const string SHA256 = "SHA256";
        private const string SECURITYCODEPADDING = "1qaw3+{:P)";
        //// security code
        private const int AllowSecurityCodeLength = 6;
        private const string SecurityCodeRule = @"^([0-9])(?!\1+$)[0-9]+$";

        private readonly SecurityCodeDataAccess _securityCodeDataAccess;

        public SecurityCodeServiceImpl(SecurityCodeDataAccess securityCodeDataAccess = null)
        {
            _securityCodeDataAccess = securityCodeDataAccess ;
        }

        public bool IsIgnoreSecurityCode(string url)
        {
            return SecurityCodeConfig.IsIgnoreSecurityCode(url);
        }

        public bool IsForceUseSecurityCode(string site, SecurityCodeStatus securityCodeResult, ISecurityCodeSession securityCodeSession)
        {
            if (SecurityCodeConfig.IsActive)
            {
                return false;
            }

            if (site == null)
            {
                throw new ArgumentNullException(nameof(site), "Site cannot be null");
            }

            var isNonUseSecurityCodeSite = AppSettings.NonUseSecurityCodeSites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str.ToLower())
                .Contains(site.ToLower());

            if (isNonUseSecurityCodeSite)
            {
                return false;
            }

            if (securityCodeSession == null)
            {
                throw new ArgumentNullException(nameof(securityCodeSession), "Security Code Session cannot be null");
            }

            bool isForceUseSecurityCode = securityCodeSession.IsRequireSecurityCode ||
               securityCodeResult == SecurityCodeStatus.FORCED_CHANGE_SC ||
               (securityCodeResult == SecurityCodeStatus.PROVIDED_SC_SUCCESS && !securityCodeSession.IsInternal);

            return isForceUseSecurityCode;
        }

        /// <summary>
        /// Checks the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="randomKey">The random key.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <returns>The result object.</returns>
        public Result CheckSecurityCode(int custId, string securityCode, string randomKey, string language, string clientIP, int subAccountId = 0)
        {
            if (string.IsNullOrEmpty(securityCode) || string.IsNullOrEmpty(randomKey))
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.alertInputWrongSecCode };
            }

            string securityCodeDB = GetSecurityCode(custId, subAccountId);
            string securityCodeDBHash = Cryptography.ComputeHash(SHA256, securityCodeDB, randomKey);
            bool validSecurityCode = string.Equals(securityCode, securityCodeDBHash);
            //// update status of Security Code
            Result result = UpdateSecurityCodeStatus(custId, validSecurityCode, language, clientIP, subAccountId);
            if (result.ErrorCode == 200)
            {
                result.ErrorCode = 2; //// account is closed
                if (subAccountId == 0)
                {
                   // new CasinoLibrary.SBAgent.Customer().UpdateCustClosedFailSecurityCode(custId, true, clientIP);
                }
            }
            else if (result.ErrorCode != 0)
            {
                result.ErrorCode = 9009; //// wrong SC
                result.ErrorMessage = SecurityCode_Resources.alertInputWrongSecCode;
            }

            return result;
        }

        /// <summary>
        /// Changes the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="oldSecurityCode">The old security code.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="randomKey">The random key.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <returns>The result object.</returns>
        public Result ChangeSecurityCode(
            int custId,
            int userId,
            string oldSecurityCode,
            string securityCode,
            string randomKey,
            string language,
            string clientIP,
            int subAccountId = 0)
        {
            //// check old SC is valid
            Result result = CheckSecurityCode(custId, oldSecurityCode, randomKey, language, clientIP, subAccountId);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            result = ChangeSecurityCodeWithoutValidation(custId, userId, securityCode, language, clientIP, subAccountId);
            return result;
        }

        /// <summary>
        /// Changes the security code without validation.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <returns>The result object.</returns>
        public Result ChangeSecurityCodeWithoutValidation(
            int custId,
            int userId,
            string securityCode,
            string language,
            string clientIP,
            int subAccountId = 0)
        {
            Result result = null;
            string hashSecurityCode = null;
            if (subAccountId > 0)
            {
                hashSecurityCode = HashSecurityCode(subAccountId, securityCode);
                result = _securityCodeDataAccess.ChangeSecurityCodeOfSubAccount(custId, userId, subAccountId, hashSecurityCode, language, clientIP);
            }
            else
            {
                hashSecurityCode = HashSecurityCode(custId, securityCode);
                //// check new SC must be totally different from old SC
                string securityCodeDB = GetSecurityCode(custId);
                if (string.Equals(hashSecurityCode, securityCodeDB))
                {
                    return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.alertSameSecCode };
                }

                result = _securityCodeDataAccess.ChangeSecurityCodeOfMainAccount(custId, userId, subAccountId, hashSecurityCode, language, clientIP);
            }

            if (result != null && result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        /// <summary>
        /// Resets the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <param name="isResetMember">If set to <c>true</c> [is reset member].</param>
        /// <returns>The result object.</returns>
        public Result ResetSecurityCode(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP,
            bool isResetMember = false)
        {
            string hashSecurityCode = HashSecurityCode(custId, securityCode, isResetMember);
            Result result = _securityCodeDataAccess.ChangeSecurityCodeOfMainAccount(
                custId,
                userId,
                subAccountId,
                hashSecurityCode,
                language,
                clientIP);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        /// <summary>
        /// Resets the security code of sub account.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <returns>The result object.</returns>
        public Result ResetSecurityCodeOfSubAccount(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP)
        {
            string hashSecurityCode = HashSecurityCode(subAccountId, securityCode);
            Result result = _securityCodeDataAccess.ChangeSecurityCodeOfSubAccount2(
                custId,
                userId,
                subAccountId,
                hashSecurityCode,
                language,
                clientIP);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        /// <summary>
        /// Validates the security code.
        /// </summary>
        /// <param name="securityCode">The security code.</param>
        /// <returns>True if security code is valid otherwise false.</returns>
        public bool ValidateSecurityCode(string securityCode)
        {
            int securityCodeLength = securityCode == null ? 0 : securityCode.Trim().Length;
            if (securityCodeLength != AllowSecurityCodeLength)
            {
                return false;
            }

            Regex regex = new Regex(SecurityCodeRule);
            //// check digits and all of them are not the same
            bool result = regex.IsMatch(securityCode);
            if (result)
            {
                //// check 'Forward Run' and 'Backward Run'
                bool isForwardRun = true, isBackwardRun = true;
                int forward = 0, backward = 0;
                forward = backward = int.Parse(securityCode[0].ToString());
                for (int i = 1; i < securityCodeLength; i++)
                {
                    int current = int.Parse(securityCode[i].ToString());
                    if (++forward != current)
                    {
                        isForwardRun = false;
                    }

                    if (--backward != current)
                    {
                        isBackwardRun = false;
                    }
                }

                result = !(isForwardRun || isBackwardRun);
            }

            return result;
        }

        public string MD5Salt(int custId)
        {
            string plainText = custId.ToString();
            return Cryptography.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

        #region Helper methods

        /// <summary>
        /// Generates the security token.
        /// </summary>
        /// <param name="size">The size of string.</param>
        /// <returns>The random string .</returns>
        public string GenerateSecurityToken(int size = 6)
        {
            return Cryptography.RandomString(size);
        }

        /// <summary>
        /// Hashes the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="isResetMember">If set to <c>true</c> [is reset member].</param>
        /// <returns>The encryption string.</returns>
        private string HashSecurityCode(int custId, string securityCode, bool isResetMember = false)
        {
            //// because the Member site doesn't want to edit his code.
            //// so that we must use the CfsEnCode method.
            if (isResetMember)
            {
                return Cryptography.CfsEnCode(securityCode);
            }

            string salt = MD5Salt(custId);
            return Cryptography.ComputeHash(SHA1, securityCode, salt);
        }

        /// <summary>
        /// Gets the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <returns>The security code string.</returns>
        private string GetSecurityCode(int custId, int subAccountId = 0)
        {
            string securityCode;
            if (subAccountId == 0)
            {
                securityCode = _securityCodeDataAccess.GetSecurityCodeOfMainAccount(custId);
            }
            else
            {
                securityCode = _securityCodeDataAccess.GetSecurityCodeOfSubAccount(custId, subAccountId);
            }

            return securityCode;
        }

        /// <summary>
        /// Updates the security code status.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="validSecurityCode">If set to <c>true</c> [valid security code].</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <returns>The result object.</returns>
        private Result UpdateSecurityCodeStatus(int custId, bool validSecurityCode, string language, string clientIP, int subAccountId = 0)
        {
            Result result = null;
            if (subAccountId == 0)
            {
                result = _securityCodeDataAccess.UpdateSecurityCodeStatusOfMainAccount(custId, validSecurityCode, language, clientIP);
            }
            else
            {
                result = _securityCodeDataAccess.UpdateSecurityCodeStatusOfSubAccount(custId, subAccountId, validSecurityCode, language, clientIP);
            }

            return result;
        }

        #endregion
    }
}