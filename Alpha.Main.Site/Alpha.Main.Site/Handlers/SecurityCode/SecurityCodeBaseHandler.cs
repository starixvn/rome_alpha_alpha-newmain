namespace Alpha.Main.Site.Handlers.SecurityCode
{
    using AppResources;
    using Fanex.Security;
    using Main.Models.Enumerations;
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

        public SecurityCodeBaseHandler(
           SecurityCodeService securityCodeService, ISecurityCodeSession securityCodeSession = null)
        {
            this.securityCodeService = securityCodeService;
            this.securityCodeSession = securityCodeSession;
        }

        private void ResetToken()
        {
            this.securityCodeSession.SecurityCodeToken = string.Empty;
            this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SECURITYCODE_SUCCESS;
        }

        private Result UpdateSecurityCode(SecurityCodeParams securityCodeParams)
        {
            Result result = securityCodeParams.SubAccountId.Equals(0) ? this.UpdateMainAccountSecurityCode(securityCodeParams) : this.UpdateSubAccountSecurityCode(securityCodeParams);

            if (result != null && result.ErrorCode == 0)
            {
                result.ErrorMessage = SecurityCode_Resources.alertSecCodeChangedOK;
            }

            return result;
        }

        private Result UpdateSubAccountSecurityCode(SecurityCodeParams securityCodeParams)
        {
            string hashSecurityCode = HashSecurityCode(securityCodeParams.SubAccountId, securityCodeParams.SecurityCode);
            securityCodeParams.HashSecurityCode = hashSecurityCode;
            return this.securityCodeService.UpdateSubAccountHashSecurityCode(securityCodeParams);
        }

        private Result UpdateMainAccountSecurityCode(SecurityCodeParams securityCodeParams)
        {
            string hashSecurityCode = HashSecurityCode(securityCodeParams.CustId, securityCodeParams.SecurityCode);
            securityCodeParams.HashSecurityCode = hashSecurityCode;
            return this.securityCodeService.UpdateMainAccountHashSecurityCode(securityCodeParams);
        }

        private Result CompareHashSecurityCodeBetweenWebAndDB(int custId, int subAccId, string webHashSecurityCode)
        {
            if (string.IsNullOrEmpty(webHashSecurityCode) || string.IsNullOrEmpty(this.securityCodeSession.SecurityCodeToken))
            {
                return new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.alertInputWrongSecCode };
            }

            string dBsecurityCode = GetDBhashSecurityCode(custId, subAccId);
            string dBHashsecurityCode = Cryptography.ComputeHash(SHA256, dBsecurityCode, this.securityCodeSession.SecurityCodeToken);
            bool isValidSecurityCode = string.Equals(webHashSecurityCode, dBHashsecurityCode);

            //// update status of Security Code
            Result result = UpdateSecurityCodeCounter(custId, isValidSecurityCode, this.securityCodeSession.Language, this.ClientIP, subAccId);
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

        private string GetDBhashSecurityCode(int custId, int subAccountId)
        {
            return subAccountId.Equals(0) ? this.securityCodeService.GetMainAccountHashSecurityCode(custId) : this.securityCodeService.GetSubAccountHashSecurityCode(custId, subAccountId);
        }

        private Result UpdateSecurityCodeCounter(int custId, bool isValidSecurityCode, string language, string clientIP, int subAccountId)
        {
            return subAccountId.Equals(0) ? this.securityCodeService.UpdateMainAccountHashSecurityCodeCounter(new SecurityCodeParams
            {
                CustId = custId,
                IsValidSecurityCode = isValidSecurityCode,
                IP = clientIP,
                Language = language
            }) : this.securityCodeService.UpdateSubAccountHashSecurityCodeCounter(new SecurityCodeParams
            {
                CustId = custId,
                SubAccountId = subAccountId,
                IsValidSecurityCode = isValidSecurityCode,
                IP = clientIP,
                Language = language
            });
        }

        protected Result ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand message)
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

        protected Result ChangeSecurityCodeByForcing(ChangeSecurityCodeByForcingCommand message)
        {
            Result result = this.CompareHashSecurityCodeBetweenWebAndDB(this.securityCodeSession.CustId, this.securityCodeSession.SubAccId, message.OldSecurityCode);
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

        protected Result ChangeUserOwnSecurityCode(ChangeUserOwnSecurityCodeCommand message)
        {
            Result result = this.CompareHashSecurityCodeBetweenWebAndDB(this.securityCodeSession.CustId, this.securityCodeSession.SubAccId, message.OldSecurityCode);
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

        protected Result VerifySecurityCode(VerifySecurityCodeCommand message)
        {
            Result result =  this.CompareHashSecurityCodeBetweenWebAndDB(message.CustId, message.SubAccId, message.SecurityCode);
            if (result.ErrorCode == 0)
            {
                this.ResetToken();
            }

            return result;
        }

        protected Result ResetDownlineSecurityCode(ResetSecurityCodeCommand securityCodeParams)
        {
            var isResetMember = this.securityCodeSession.UserLevel.Equals(UserLevel.Agent);
            string hashSecurityCode = HashSecurityCode(securityCodeParams.CustId.Value, securityCodeParams.SecurityCode, isResetMember);

            return this.securityCodeService.UpdateMainAccountHashSecurityCode(new SecurityCodeParams
            {
                CustId = securityCodeParams.CustId.Value,
                SubAccountId = securityCodeParams.SubAccId.Value,
                UserId = securityCodeParams.UserId,
                SecurityCode = hashSecurityCode,
                Language = securityCodeParams.Language,
                IP = this.ClientIP
            });
        }

        protected Result ResetSubAccountSecurityCode(ResetSecurityCodeCommand securityCodeParams)
        {
            //Reset SubAccount SecurityCode By Upline Or MainAccount
            string hashSecurityCode = HashSecurityCode(securityCodeParams.SubAccId.Value, securityCodeParams.SecurityCode);
            Result result = this.securityCodeService.ResetSubAccountHashSecurityCode(new SecurityCodeParams
            {
                CustId = securityCodeParams.CustId.Value,
                SubAccountId = securityCodeParams.SubAccId.Value,
                UserId = securityCodeParams.UserId,
                HashSecurityCode = securityCodeParams.SecurityCode,
                IP = this.ClientIP,
                Language = securityCodeParams.Language
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
      }
}