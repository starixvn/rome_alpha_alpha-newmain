namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class PasswordDataAccessImpl : PasswordDataAccess
    {
        public Result UpdatePassword(ICriteria criteria)
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