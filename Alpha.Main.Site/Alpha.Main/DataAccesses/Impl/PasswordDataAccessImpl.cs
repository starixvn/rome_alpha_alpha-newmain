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

                var code = (int)outputParams["errcode"];

                return new Result
                {
                    Code = code,
                    Messsage = code == 0 
                        ? AppResources.Resources.UpdatedSuccessfully 
                        : outputParams["errmsg"].ToString()
                };
            }
        }
    }
}