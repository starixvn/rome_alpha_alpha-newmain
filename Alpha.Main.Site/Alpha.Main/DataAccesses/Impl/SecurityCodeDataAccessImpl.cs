namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using Alpha.Main.DataAccesses;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class SecurityCodeDataAccessImpl : BaseDataAccessImpl, SecurityCodeDataAccess
    {
        public string GetSecurityCode(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
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
            return RunUpdateCommand(criteria);
        }
    }
}