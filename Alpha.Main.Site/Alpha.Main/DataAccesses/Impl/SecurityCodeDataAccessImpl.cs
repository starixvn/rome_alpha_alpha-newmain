namespace Alpha.Main.DataAccesses.Impl
{
    using Alpha.Main.DataAccesses;
    using Alpha.Main.Models.SecurityCode;
    using Criteria.SecurityCode;
    using Fanex.Common;
    using Fanex.Data;
    using System;
    using System.Collections;
    using System.Data;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SecurityCodeDataAccessImpl : SecurityCodeDataAccess
    {
        public string GetSecurityCodeOfMainAccount(GetSecurityCodeOfMainAccountCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());
                if (reader.Read())
                {
                    return reader.GetString(0);
                }
            }

            return string.Empty;
        }

        public string GetSecurityCodeOfSubAccount(GetSecurityCodeOfSubAccountCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_CustomerSelSecurityCode_Sub"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", criteria.CustId);
                inputParams.Add("subaccid", criteria.SubaccId);

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

        public Result UpdateSecurityCodeStatusOfMainAccount(UpdateSecurityCodeStatusOfMainAccountCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCodeStatus"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", criteria.CustId);
                inputParams.Add("checkedsecuritycode", criteria.CheckedSecurityCode);
                inputParams.Add("language", criteria.Language);
                inputParams.Add("ip", criteria.IP);
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

        public Result UpdateSecurityCodeStatusOfSubAccount(UpdateSecurityCodeStatusOfSubAccountCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCodeStatus_Sub"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", criteria.CustId);
                inputParams.Add("subaccid", criteria.SubaccId);
                inputParams.Add("checkedsecuritycode", criteria.CheckedSecurityCode);
                inputParams.Add("language", criteria.Language);
                inputParams.Add("ip", criteria.IP);
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

        public Result ChangeSecurityCodeOfMainAccount(ChangeSecurityCodeOfMainAccountCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCode"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", criteria.CustId);
                inputParams.Add("userid", criteria.UserId);
                inputParams.Add("subaccid", criteria.SubaccId);
                inputParams.Add("securitycode", criteria.HashSecurityCode);
                inputParams.Add("language", criteria.Language);
                inputParams.Add("ip", criteria.IP);
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

        public Result ChangeSecurityCodeOfSubAccount(ChangeSecurityCodeOfSubAccountCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCode_Sub"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", criteria.CustId);
                inputParams.Add("userid", criteria.UserId);
                inputParams.Add("subaccid", criteria.SubaccId);
                inputParams.Add("securitycode", criteria.HashSecurityCode);
                inputParams.Add("language", criteria.Language);
                inputParams.Add("ip", criteria.IP);
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

        public Result ChangeSecurityCodeOfSubAccount2(ChangeSecurityCodeOfSubAccount2Criteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_CustomerUpdSecurityCode_Sub2"))
            {
                Hashtable inputParams = new Hashtable();
                inputParams.Add("custid", criteria.CustId);
                inputParams.Add("userid", criteria.UserId);
                inputParams.Add("subaccid", criteria.SubaccId);
                inputParams.Add("securitycode", criteria.HashSecurityCode);
                inputParams.Add("language", criteria.Language);
                inputParams.Add("ip", criteria.IP);
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
    }
}