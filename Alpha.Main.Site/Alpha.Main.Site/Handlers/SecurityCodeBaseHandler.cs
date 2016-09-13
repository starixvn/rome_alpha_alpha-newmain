namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using AppResources;
    using Encryption;
    using Fanex.Security;
    using Main.Models.SecurityCode;
    using Models.Request.SecurityCode;
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

        private void ResetToken()
        {
            this.securityCodeSession.SecurityCodeToken = string.Empty;
            this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
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

        protected Result ChangeSecurityCodeFirstLogin(ForceChangeSecurityCodeTheFirstLoginCommand message)
        {
            return this.UpdateSecurityCode(new SecurityCodeParams
            {
                CustId = message.CustId,
                SubAccountId = message.SubAccId,
                UserId = message.UserId,
                SecurityCode = message.SecurityCode,
                Language = message.Language,
                IP = this.ClientIP
            });
        }

        protected Result ForceChangeSecurityCodeWhenLogin(ForceChangeSecurityCodeWhenLoginCommand message)
        {
            Result result = this.CompareSecurityCodeBetweenWebAndDB(this.securityCodeSession.CustId, this.securityCodeSession.SubAccId, message.OldSecurityCode);
            if (result.ErrorCode.Equals(0))
            {
                result = this.UpdateSecurityCode(new SecurityCodeParams
                {
                    CustId = message.CustId,
                    SubAccountId = message.SubAccId,
                    UserId = message.UserId,
                    SecurityCode = message.SecurityCode,
                    Language = message.Language,
                    IP = this.ClientIP
                });
            }

            if (result.ErrorCode.Equals(0))
            {
                this.ResetToken();
            }

            return result;
        }

        protected Result ChangeSecurityCodeByItself(ChangeSecurityCodeByItselfCommand message)
        {
            Result result = this.CompareSecurityCodeBetweenWebAndDB(this.securityCodeSession.CustId, this.securityCodeSession.SubAccId, message.OldSecurityCode);
            if (result.ErrorCode.Equals(0))
            {
                result =  this.UpdateSecurityCode(new SecurityCodeParams
                {
                    CustId = message.CustId,
                    SubAccountId = message.SubAccId,
                    UserId = message.UserId,
                    SecurityCode = message.SecurityCode,
                    Language = message.Language,
                    IP = this.ClientIP
                });
            }

            if (result.ErrorCode.Equals(0))
            {
                this.ResetToken();
            }

            return result;
        }

        protected Result VerifySecurityCodeForImportantAction(VerifySecurityCodeCommand message)
        {
            return this.CompareSecurityCodeBetweenWebAndDB(message.CustId, message.SubAccountId, message.SecurityCode);
        }
     
        private Result CompareSecurityCodeBetweenWebAndDB(int custId, int subAccId, string securityCode)
        {
            if (string.IsNullOrEmpty(securityCode) || string.IsNullOrEmpty(this.securityCodeSession.SecurityCodeToken))
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.alertInputWrongSecCode };
            }

            string securityCodeDB = GetSecurityCode(custId, subAccId);
            string securityCodeDBHash = Cryptography.ComputeHash(SHA256, securityCodeDB, this.securityCodeSession.SecurityCodeToken);
            bool validSecurityCode = string.Equals(this.securityCodeSession.SecurityCode, securityCodeDBHash);

            //// update status of Security Code
            Result result = UpdateSecurityCodeCounter(custId, validSecurityCode, this.securityCodeSession.Language, this.ClientIP, subAccId);
            if (result.ErrorCode == 200)
            {
                result.ErrorCode = 2; //// account is closed
                if (this.securityCodeSession.SubAccId == 0)
                {
                    this.CloseCasinoAccount();
                }
            }
            else if (result.ErrorCode != 0)
            {
                result.ErrorCode = 9009; //// wrong SC
                result.ErrorMessage = SecurityCode_Resources.alertInputWrongSecCode;
            }

            return result;
        }

        private void CloseCasinoAccount()
        {
           // new CasinoLibrary.SBAgent.Customer().UpdateCustClosedFailSecurityCode(custId, true, clientIP);
        }

        protected Result UpdateSecurityCode(SecurityCodeParams inputParams)
        {
            Result result = inputParams.SubAccountId.Equals(0) ? this.UpdateMainAccountSecurityCode(inputParams) : this.UpdateSubAccountSecurityCode(inputParams);

            if (result != null && result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }
        private Result UpdateSubAccountSecurityCode(SecurityCodeParams inputParams)
        {
            string hashSecurityCode = HashSecurityCode(inputParams.SubAccountId, inputParams.SecurityCode);
            inputParams.HashSecurityCode = hashSecurityCode;
            return this.securityCodeService.UpdateSubAccountSecurityCode(inputParams);
        }
        private Result UpdateMainAccountSecurityCode(SecurityCodeParams inputParams)
        {
            string hashSecurityCode = HashSecurityCode(inputParams.CustId, inputParams.SecurityCode);
            inputParams.HashSecurityCode = hashSecurityCode;
            return this.securityCodeService.UpdateMainAccountSecurityCode(inputParams);
        }

        protected Result ResetDownlineSecurityCode(ResetSecurityCodeCommand message)
        {
            var isResetMember = this.securityCodeSession.RoleId == (byte)UserRole.Agent;
            string hashSecurityCode = HashSecurityCode(message.CustId.Value, message.SecurityCode, isResetMember);

            return this.securityCodeService.UpdateMainAccountSecurityCode(new SecurityCodeParams
            {
                CustId = message.CustId.Value,
                SubAccountId = message.SubAccId.Value,
                UserId = message.UserId,
                SecurityCode = hashSecurityCode,
                Language = message.Language,
                IP = this.ClientIP
            });
        }

        protected Result ResetSubAccountSecurityCode(ResetSecurityCodeCommand message)
        {
            //Reset SubAccount SecurityCode By Upline Or MainAccount
            string hashSecurityCode = HashSecurityCode(message.SubAccId.Value, message.SecurityCode);
            Result result = this.securityCodeService.ResetSubAccountSecurityCode(new SecurityCodeParams
            {
                CustId = message.CustId.Value,
                SubAccountId = message.SubAccId.Value,
                UserId = message.UserId,
                HashSecurityCode = message.SecurityCode,
                IP = this.ClientIP,
                Language = message.Language
            });

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        protected bool ValidateSecurityCode(string securityCode)
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

        protected string MD5Salt(int custId)
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
            return subAccountId.Equals(0) ? this.securityCodeService.GetMainAccountSecurityCode(custId) : this.securityCodeService.GetSubAccountSecurityCode(custId, subAccountId);
        }

        private Result UpdateSecurityCodeCounter(int custId, bool validSecurityCode, string language, string clientIP, int subAccountId)
        {
           return subAccountId.Equals(0) ? this.securityCodeService.UpdateMainAccountSecurityCodeCounter(new SecurityCodeParams
           {
               CustId = custId,
               IsValidSecurityCode = validSecurityCode,
               IP = clientIP,
               Language = language
           }) : this.securityCodeService.UpdateSubAccountSecurityCodeCounter(new SecurityCodeParams
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