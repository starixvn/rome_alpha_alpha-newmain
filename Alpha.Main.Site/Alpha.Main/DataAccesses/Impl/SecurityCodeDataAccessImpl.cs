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
        public string GetMainAccountHashSecurityCode(GetMainAccountHashSecurityCodeCriteria criteria)
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

        public string GetSubAccountHashSecurityCode(GetSubAccountHashSecurityCodeCriteria criteria)
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

        public Result UpdateMainAccountSecurityCodeCounter(UpdateMainAccountSecurityCodeCounterCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_UpdateMainAccountSecurityCodeCounter"))
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

        public Result UpdateSubAccountSecurityCodeCounter(UpdateSubAccountSecurityCodeCounterCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_UpdateSubAccountSecurityCodeCounter"))
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

        public Result UpdateMainAccountHashSecurityCode(UpdateMainAccountHashSecurityCodeCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_UpdateMainAccountSecurityCode"))
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

        public Result UpdateSubAccountHashSecurityCode(UpdateSubAccountHashSecurityCodeCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_UpdateSubAccountSecurityCode"))
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

        public Result ResetSubAccountHashSecurityCode(ResetSubAccountHashSecurityCodeCriteria criteria)
        {
            using (var objectDb = new ObjectDb("Age_ResetSubAccountSecurityCode"))
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