namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using System.Collections;
    using Alpha.Main.DataAccesses;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class SecurityCodeDataAccessImpl : SecurityCodeDataAccess
    {
        public string GetSecurityCode(ICriteria criteria)
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

        public Result UpdateSecurityCode(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                var code = (int)outputParams["errcode"];

                return new Result
                {
                    Code = code,
                    Message = code == 0
                        ? AppResources.Resources.UpdatedSuccessfully
                        : outputParams["errmsg"].ToString()
                };
            }
        }
    }
}