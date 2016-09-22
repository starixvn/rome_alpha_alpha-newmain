namespace Alpha.Main.DataAccesses.Impl
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class PasswordDataAccessImpl : PasswordDataAccess
    {
        public Result UpdatePassword(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result
                {
                    Code = (int)outputParams["errcode"],
                    Messsage = outputParams["errmsg"].ToString()
                };
            }
        }
    }
}