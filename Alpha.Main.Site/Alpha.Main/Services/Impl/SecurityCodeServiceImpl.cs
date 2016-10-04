namespace Alpha.Main.Services.Impl
{
    using System.Text.RegularExpressions;
    using Alpha.Main.Services;
    using AppResources;
    using Criteria.SecurityCode;
    using DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Security;
    using Models;
    using Params.SecurityCode;

    public class SecurityCodeServiceImpl : SecurityCodeService
    {
        private const string MD5 = "MD5";
        private const string SHA1 = "SHA1";
        private const string SHA256 = "SHA256";
        private const string SECURITYCODEPADDING = "1qaw3+{:P)";
        private const int AllowSecurityCodeLength = 6;
        private const string SecurityCodeRule = @"^([0-9])(?!\1+$)[0-9]+$";

        private readonly SecurityCodeDataAccess securityCodeDataAccess;

        public SecurityCodeServiceImpl(SecurityCodeDataAccess securityCodeDataAccess = null)
        {
            this.securityCodeDataAccess = securityCodeDataAccess ?? new SecurityCodeDataAccessImpl();
        }

        private string ClientIP
        {
            get
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                    ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string[] ips = string.IsNullOrEmpty(ip) ? new string[] { string.Empty } : ip.Split(',');

                return ips[0];
            }
        }

        public string GetMainAccountHashSecurityCode(int custId)
        {
            return this.securityCodeDataAccess.GetHashSecurityCode(
                new GetMainAccountHashSecurityCodeCriteria
                {
                    CustId = custId
                });
        }

        public string GetSubAccountHashSecurityCode(int custId, int subAccountId)
        {
            return this.securityCodeDataAccess.GetHashSecurityCode(
                new GetSubAccountHashSecurityCodeCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId
                });
        }

        public Result UpdateMainAccountHashSecurityCode(SecurityCodeParams securityCodeParams)
        {
            string hashSecurityCode = this.HashSecurityCode(securityCodeParams.CustId, securityCodeParams.SecurityCode, securityCodeParams.IsResetMember);

            return this.securityCodeDataAccess.UpdateSecurityCode(
                 new UpdateMainAccountHashSecurityCodeCriteria
                 {
                     CustId = securityCodeParams.CustId,
                     SubaccId = securityCodeParams.SubAccId,
                     UserId = securityCodeParams.UserId,
                     HashedSecurityCode = hashSecurityCode,
                     IP = this.ClientIP,
                     Language = securityCodeParams.Language
                 });
        }

        public Result UpdateSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams)
        {
            string hashSecurityCode = this.HashSecurityCode(securityCodeParams.SubAccId, securityCodeParams.SecurityCode);

            return this.securityCodeDataAccess.UpdateSecurityCode(new UpdateSubAccountHashSecurityCodeCriteria
            {
                CustId = securityCodeParams.CustId,
                SubaccId = securityCodeParams.SubAccId,
                UserId = securityCodeParams.UserId,
                HashedSecurityCode = hashSecurityCode,
                IP = this.ClientIP,
                Language = securityCodeParams.Language
            });
        }

        public Result ResetSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams)
        {
            string hashSecurityCode = this.HashSecurityCode(securityCodeParams.SubAccId, securityCodeParams.SecurityCode);
            return this.securityCodeDataAccess.UpdateSecurityCode(new ResetSubAccountHashSecurityCodeCriteria
            {
                CustId = securityCodeParams.CustId,
                SubaccId = securityCodeParams.SubAccId,
                UserId = securityCodeParams.UserId,
                HashedSecurityCode = hashSecurityCode,
                IP = this.ClientIP,
                Language = securityCodeParams.Language
            });
        }

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

        public string MD5Salt(int custId)
        {
            string plainText = custId.ToString();
            return Cryptography.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

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

        public Result CompareHashSecurityCodeBetweenWebAndDB(int custId, int subAccId, bool isSubAcc, string securityCodeToken, string language,  string webHashSecurityCode)
        {
            if (string.IsNullOrEmpty(webHashSecurityCode) || string.IsNullOrEmpty(securityCodeToken))
            {
                return new Result { Code = -1, Messsage = Security.alertInputWrongSecCode };
            }

            string securityCodeDB = GetDBhashSecurityCode(custId, subAccId);
            string hashsecurityCodeDB = Cryptography.ComputeHash(SHA256, securityCodeDB, securityCodeToken);
            bool isValidSecurityCode = string.Equals(webHashSecurityCode, hashsecurityCodeDB);

            //// update status of Security Code
            Result result = UpdateSecurityCodeCounter(custId, isValidSecurityCode, language, subAccId);
            if (result.Code == 200)
            {
                result.Code = 2; //// account is closed
                if (isSubAcc)
                {
                    this.CloseCasinoAccount(custId, ClientIP);
                }
            }
            else if (result.Code != 0)
            {
                result.Code = 9009; //// wrong SC
                result.Messsage = Security.alertInputWrongSecCode;
            }

            return result;
        }

        private void CloseCasinoAccount(int custId, string ip)
        {
            new CasinoLibrary.SBAgent.Customer().UpdateCustClosedFailSecurityCode(custId, true, ip);
        }

        private string GetDBhashSecurityCode(int custId, int subAccountId)
        {
            return subAccountId.Equals(0) ? this.GetMainAccountHashSecurityCode(custId)
                : this.GetSubAccountHashSecurityCode(custId, subAccountId);
        }

        private Result UpdateSecurityCodeCounter(int custId, bool isValidSecurityCode, string language, int subAccountId)
        {
            return subAccountId.Equals(0) ?
                this.securityCodeDataAccess.UpdateSecurityCode(
                new UpdateMainAccountSecurityCodeCounterCriteria
                {
                    CustId = custId,
                    CheckedSecurityCode = isValidSecurityCode,
                    IP = this.ClientIP,
                    Language = language
                })
                : this.securityCodeDataAccess.UpdateSecurityCode(
                new UpdateSubAccountSecurityCodeCounterCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId,
                    CheckedSecurityCode = isValidSecurityCode,
                    IP = this.ClientIP,
                    Language = language
                });
        }
    }
}