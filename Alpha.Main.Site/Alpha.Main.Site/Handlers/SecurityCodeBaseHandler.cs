namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using AppResources;
    using Encryption;
    using Fanex.Security;
    using Main.Models.SecurityCode;
    using Params.SecurityCode;
    using Services;
    using Sessions;
    using System.Text.RegularExpressions;
    public class SecurityCodeBaseHandler 
    {
        private const string MD5 = "MD5";
        private const string SHA1 = "SHA1";
        private const string SHA256 = "SHA256";
        private const string SECURITYCODEPADDING = "1qaw3+{:P)";
        private const int AllowSecurityCodeLength = 6;
        private const string SecurityCodeRule = @"^([0-9])(?!\1+$)[0-9]+$";

        protected ISecurityCodeSession securityCodeSession;
        private SecurityCodeService securityCodeService;

        protected string PH
        {
            get
            {
                return System.Web.HttpContext.Current.Session["ph"] + string.Empty;
            }
        }

        protected string ClientIP
        {
            get
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                    ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string[] ips = string.IsNullOrEmpty(ip) ? new string[] { string.Empty } : ip.Split(',');

                return ips[0];
            }
        }

        public SecurityCodeBaseHandler(
           SecurityCodeService securityCodeService, ISecurityCodeSession securityCodeSession = null)
        {
            this.securityCodeService = securityCodeService;
            this.securityCodeSession = securityCodeSession;
        }

        protected string GenerateSecurityToken(int size = 6)
        {
            return Cryptography.RandomString(size);
        }

        protected string GetRC4Decryption(string encryption)
        {
            if (string.IsNullOrEmpty(encryption))
            {
                return string.Empty;
            }

            return Encryptor.RC4(this.PH, Encryptor.HexDecode(encryption));
        }

        public Result CompareSecurityCodeBetweenWebAndDB(CheckSecurityCodeParams inputParams)
        {
            if (string.IsNullOrEmpty(inputParams.SecurityCode) || string.IsNullOrEmpty(inputParams.SecurityCodeToken))
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.alertInputWrongSecCode };
            }

            string securityCodeDB = GetSecurityCode(inputParams.CustId, inputParams.SubAccountId);
            string securityCodeDBHash = Cryptography.ComputeHash(SHA256, securityCodeDB, inputParams.SecurityCodeToken);
            bool validSecurityCode = string.Equals(inputParams.SecurityCode, securityCodeDBHash);

            //// update status of Security Code
            Result result = UpdateSecurityCodeCounter(inputParams.CustId, validSecurityCode, inputParams.Language, inputParams.IP, inputParams.SubAccountId);
            if (result.ErrorCode == 200)
            {
                result.ErrorCode = 2; //// account is closed
                if (inputParams.SubAccountId == 0)
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

        public Result ForceChangeSecurityCodeByFirstLogin(SecurityCodeParams inputParams)
        {

            ////// check old SC is valid
            //Result result = CheckSecurityCode(inputParams);

            //if (result.ErrorCode != 0)
            //{
            //    return result;
            //}

            Result result = ChangeSecurityCode(inputParams);

            return result;
        }

        public Result ChangeSecurityCode(SecurityCodeParams inputParams)
        {
            Result result = null;
            string hashSecurityCode = null;
            if (inputParams.SubAccountId > 0)
            {
                hashSecurityCode = HashSecurityCode(inputParams.SubAccountId, inputParams.SecurityCode);
                inputParams.HashSecurityCode = hashSecurityCode;

                result = this.securityCodeService.UpdateSubAccountSecurityCode(inputParams);
            }
            else
            {
                hashSecurityCode = HashSecurityCode(inputParams.CustId, inputParams.SecurityCode);
                inputParams.HashSecurityCode = hashSecurityCode;
                //// check new SC must be totally different from old SC
                string securityCodeDB = this.GetSecurityCode(inputParams.CustId, inputParams.SubAccountId);
                if (string.Equals(hashSecurityCode, securityCodeDB))
                {
                    return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.alertSameSecCode };
                }

                result = this.UpdateMainAccountSecurityCode(inputParams);
            }

            if (result != null && result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        public Result UpdateMainAccountSecurityCode(SecurityCodeParams inputParams)
        {
            string hashSecurityCode = HashSecurityCode(inputParams.CustId, inputParams.SecurityCode, inputParams.IsResetMember);
            Result result = this.securityCodeService.UpdateMainAccountSecurityCode(new SecurityCodeParams
            {
                CustId = inputParams.CustId,
                SubAccountId = inputParams.SubAccountId,
                UserId = inputParams.UserId,
                HashSecurityCode = inputParams.HashSecurityCode,
                IP = inputParams.IP,
                Language = inputParams.Language
            });

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        public Result ResetSubAccountSecurityCodeByUplineOrMainAccount(SecurityCodeParams inputParams)
        {
            string hashSecurityCode = HashSecurityCode(inputParams.SubAccountId, inputParams.SecurityCode);
            Result result = this.securityCodeService.ResetSubAccountSecurityCode(new SecurityCodeParams
            {
                CustId = inputParams.CustId,
                SubAccountId = inputParams.SubAccountId,
                UserId = inputParams.UserId,
                HashSecurityCode = inputParams.SecurityCode,
                IP = inputParams.IP,
                Language = inputParams.Language
            });

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
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

        public string MD5Salt(int custId)
        {
            string plainText = custId.ToString();
            return Cryptography.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
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

        private string GetSecurityCode(int custId, int subAccountId)
        {
            if (subAccountId.Equals(0))
            {
                return this.securityCodeService.GetMainAccountSecurityCode(custId);
            }
            else
            {
                return this.securityCodeService.GetSubAccountSecurityCode(custId, subAccountId);
            }
        }

        private Result UpdateSecurityCodeCounter(int custId, bool validSecurityCode, string language, string clientIP, int subAccountId)
        {
            if (subAccountId.Equals(0))
            {
                return this.securityCodeService.UpdateMainAccountSecurityCodeCounter(new SecurityCodeParams
                {
                    CustId = custId,
                    IsValidSecurityCode = validSecurityCode,
                    IP = clientIP,
                    Language = language
                });
            }
            else
            {
                return this.securityCodeService.UpdateSubAccountSecurityCodeCounter(new SecurityCodeParams
                {
                    CustId = custId,
                    SubAccountId = subAccountId,
                    IsValidSecurityCode = validSecurityCode,
                    IP = clientIP,
                    Language = language
                });
            }
        }

    }
}