namespace Alpha.Main.DataAccesses.Impl
{
    using Alpha.Main.DataAccesses;
    using Alpha.Main.Models.SecurityCode;
    using Fanex.Common;
    using Fanex.Data;
    using System.Collections;
    using System.Data;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SecurityCodeDataAccessImpl : SecurityCodeDataAccess
    {
        #region ISecurityCodeRepository Members

        /// <summary>
        /// Gets the security code of main account.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <returns>Security Code string.</returns>
        public string GetSecurityCodeOfMainAccount(int custId)
        {
            using (var objectDb = new ObjectDb("Age_CustomerSelSecurityCode"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);

                objectDb.AssignParamValues(inputParams);
                using (IDataReader securityCodeReader = objectDb.ExecuteReader())
                {
                    if (securityCodeReader.Read())
                    {
                        return securityCodeReader.GetString(0);
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the security code of sub account.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <returns>Security Code string.</returns>
        public string GetSecurityCodeOfSubAccount(int custId, int subAccountId)
        {
            using (var objectDb = new ObjectDb("Age_CustomerSelSecurityCode_Sub"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);
                inputParams.Add("subaccid", subAccountId);

                objectDb.AssignParamValues(inputParams);
                using (IDataReader securityCodeReader = objectDb.ExecuteReader())
                {
                    if (securityCodeReader.Read())
                    {
                        return securityCodeReader.GetString(0);
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Updates the security code status of main account.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="validSecurityCode">If set to <c>true</c> [valid security code].</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <returns>The result object.</returns>
        public Result UpdateSecurityCodeStatusOfMainAccount(int custId, bool validSecurityCode, string language, string clientIP)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCodeStatus"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);
                inputParams.Add("checkedsecuritycode", validSecurityCode);
                inputParams.Add("language", language);
                inputParams.Add("ip", clientIP);
                //// error output parameters
                inputParams.Add("errcode", 0);
                inputParams.Add("errmsg", string.Empty);

                objectDb.AssignParamValues(inputParams);
                int effectedRows = objectDb.ExecuteNonQuery();

                Hashtable outputParams = objectDb.GetOuputParams();
                return new Result
                {
                    ErrorCode = Converter.ToInt(outputParams["errcode"]),
                    ErrorMessage = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }

        /// <summary>
        /// Updates the security code status of sub account.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <param name="validSecurityCode">If set to <c>true</c> [valid security code].</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <returns>The result object.</returns>
        public Result UpdateSecurityCodeStatusOfSubAccount(int custId, int subAccountId, bool validSecurityCode, string language, string clientIP)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCodeStatus_Sub"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);
                inputParams.Add("subaccid", subAccountId);
                inputParams.Add("checkedsecuritycode", validSecurityCode);
                inputParams.Add("language", language);
                inputParams.Add("ip", clientIP);
                //// error output parameters
                inputParams.Add("errcode", 0);
                inputParams.Add("errmsg", string.Empty);

                objectDb.AssignParamValues(inputParams);
                int effectedRows = objectDb.ExecuteNonQuery();

                Hashtable outputParams = objectDb.GetOuputParams();
                return new Result
                {
                    ErrorCode = Converter.ToInt(outputParams["errcode"]),
                    ErrorMessage = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }

        /// <summary>
        /// Changes the security code of main account.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <returns>The result object.</returns>
        public Result ChangeSecurityCodeOfMainAccount(int custId, int userId, int subAccountId, string securityCode, string language, string clientIP)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCode"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);
                inputParams.Add("userid", userId);
                inputParams.Add("subaccid", subAccountId);
                inputParams.Add("securitycode", securityCode);
                inputParams.Add("language", language);
                inputParams.Add("ip", clientIP);
                //// error output parameters
                inputParams.Add("errcode", 0);
                inputParams.Add("errmsg", string.Empty);

                objectDb.AssignParamValues(inputParams);
                int effectedRows = objectDb.ExecuteNonQuery();

                Hashtable outputParams = objectDb.GetOuputParams();
                return new Result
                {
                    ErrorCode = Converter.ToInt(outputParams["errcode"]),
                    ErrorMessage = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }

        /// <summary>
        /// Changes the security code of sub account.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <returns>The result object.</returns>
        public Result ChangeSecurityCodeOfSubAccount(int custId, int userId, int subAccountId, string securityCode, string language, string clientIP)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCode_Sub"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);
                inputParams.Add("userid", userId);
                inputParams.Add("subaccid", subAccountId);
                inputParams.Add("securitycode", securityCode);
                inputParams.Add("language", language);
                inputParams.Add("ip", clientIP);
                //// error output parameters
                inputParams.Add("errcode", 0);
                inputParams.Add("errmsg", string.Empty);

                objectDb.AssignParamValues(inputParams);
                int effectedRows = objectDb.ExecuteNonQuery();

                Hashtable outputParams = objectDb.GetOuputParams();
                return new Result
                {
                    ErrorCode = Converter.ToInt(outputParams["errcode"]),
                    ErrorMessage = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }

        /// <summary>
        /// Changes the security code of sub account2.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="subAccountId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="language">The language.</param>
        /// <param name="clientIP">The client IP.</param>
        /// <returns>The result object.</returns>
        public Result ChangeSecurityCodeOfSubAccount2(int custId, int userId, int subAccountId, string securityCode, string language, string clientIP)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCode_Sub2"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", custId);
                inputParams.Add("userid", userId);
                inputParams.Add("subaccid", subAccountId);
                inputParams.Add("securitycode", securityCode);
                inputParams.Add("language", language);
                inputParams.Add("ip", clientIP);
                //// error output parameters
                inputParams.Add("errcode", 0);
                inputParams.Add("errmsg", string.Empty);

                objectDb.AssignParamValues(inputParams);
                int effectedRows = objectDb.ExecuteNonQuery();

                Hashtable outputParams = objectDb.GetOuputParams();
                return new Result
                {
                    ErrorCode = Converter.ToInt(outputParams["errcode"]),
                    ErrorMessage = Converter.ToString2(outputParams["errmsg"])
                };
            }
        }

        #endregion
    }
}