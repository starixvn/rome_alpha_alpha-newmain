namespace Alpha.Main.Sessions
{
    using Configurations;
    using Models.SecurityCode;
    using System.Web;

    public class SecurityCodeSession : ISecurityCodeSession
    {
        private const string HYPHEN = "_";
        //// user information
        private const string DEFAULTLANGUAGE = "en-US";
        private const string LANGUAGE = "LANGUAGE";
        private const string CUSTID = "CUSTID";
        private const string SUBACCID = "SUBACCID";
        private const string ADMINID = "ADMINID";
        private const string ROLEID = "ROLEID";
        private const string USERNAME = "USERNAME";

        //// security code
        private const string ISFORCEDSECURITYCODE = "ISFORCEDSECURITYCODE";
        private const string SECURITYCODE = "SECURITYCODE";
        private const string SECURITYCODETOKEN = "SECURITYCODETOKEN";
        private const string SECURITYCODERESULT = "SECURITYCODERESULT";

        private string _netSessionId;

        public SecurityCodeSession(string netSessionId)
        {
            this._netSessionId = netSessionId;
        }

        #region ISecurityCodeSession Members

        #region user information

        public string Language
        {
            get
            {
                object value = this.GetProperty(LANGUAGE);
                return (null == value) ? DEFAULTLANGUAGE : value as string;
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public int CustId
        {
            get
            {
                object value = this.GetProperty(CUSTID);
                return (null == value) ? 0 : (int)value;
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public int SubAccId
        {
            get
            {
                object value = this.GetProperty(SUBACCID);
                return (null == value) ? 0 : (int)value;
            }
        }

        public byte RoleId
        {
            get
            {
                object value = this.GetProperty(ROLEID);
                return (null == value) ? byte.MinValue : (byte)value;
            }
        }

        public bool IsInternal
        {
            get
            {
                return this.AdminId != 0;
            }
        }

        #endregion

        #region security code

        public bool IsForcedSecurityCode
        {
            get
            {
                object value = this.GetProperty(ISFORCEDSECURITYCODE);
                return (null == value) ? false : (bool)value;
            }
        }

        public string SecurityCode
        {
            get
            {
                return (string)this.GetProperty(SECURITYCODE);
            }
        }

        public string SecurityCodeToken
        {
            get
            {
                return (string)this.GetProperty(SECURITYCODETOKEN);
            }

            set
            {
                this.SetProperty(SECURITYCODETOKEN, value);
            }
        }

        public int SecurityCodeResult
        {
            get
            {
                // The status have not in Session yet.
                //if (!SecurityCodeConfig.IsActive)
                //{
                //    // Cannot update SC Result to NONE, because maybe we will use the old Flow
                //    return (int)SecurityCodeStatus.NONE;
                //}

                object value = this.GetProperty(SECURITYCODERESULT);
                if (value != null)
                {
                    return (int)value;
                }

                SecurityCodeStatus status;
                bool isForceSecurityCode = this.IsForcedSecurityCode;
                bool hasSC = !string.IsNullOrEmpty(this.SecurityCode);
                if (isForceSecurityCode)
                {
                    if (this.IsInternal)
                    {
                        status = SecurityCodeStatus.PROVIDED_SC_SUCCESS;
                    }
                    else
                    {
                        status = hasSC
                            ? SecurityCodeStatus.FORCED_CHANGE_SC
                            : SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION;
                    }
                }
                else
                {
                    if (hasSC)
                    {
                        status = this.IsInternal
                            ? SecurityCodeStatus.PROVIDED_SC_SUCCESS
                            : SecurityCodeStatus.PROVIDED_SC;
                    }
                    else
                    {
                        status = SecurityCodeStatus.NONE;
                    }
                }

                // only assign value when CustId != 0 (Session is initialize)
                if (this.CustId > 0)
                {
                    this.SetProperty(SECURITYCODERESULT, (int)status);
                }

                return (int)status;
            }

            set
            {
                this.SetProperty(SECURITYCODERESULT, value);
            }
        }

        #endregion

        public bool IsEnableSecurityCode
        {
            get
            {
                return this.SecurityCodeResult != (int)SecurityCodeStatus.NONE;
            }
        }

        public bool IsRequireSecurityCode
        {
            get
            {
                SecurityCodeStatus status = (SecurityCodeStatus)this.SecurityCodeResult;
                if (status == SecurityCodeStatus.NONE || status == SecurityCodeStatus.PROVIDED_SC_SUCCESS)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        private int AdminId
        {
            get
            {
                object value = this.GetProperty(ADMINID);
                return (null == value) ? 0 : (int)value;
            }
        }

        public int UserId
        {
            get
            {
                if (this.IsInternal)
                {
                    return this.AdminId;
                }
                else
                {
                    return this.CustId;
                }
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        #region helper methods

        private object GetProperty(string key)
        {
            return HttpContext.Current.Session[this._netSessionId + HYPHEN + key];
        }

        private void SetProperty(string key, object value)
        {
            HttpContext.Current.Session[this._netSessionId + HYPHEN + key] = value;
        }

        #endregion
    }
}
