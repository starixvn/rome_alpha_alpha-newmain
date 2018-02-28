namespace Alpha.Main.DataAccesses.Impl
{
    using Fanex.Data.Repository;
    using Models;

    public class PasswordDataAccessImpl : BaseDataAccessImpl, PasswordDataAccess
    {
        public Result UpdatePassword(ICriteria criteria)
        {
            return RunUpdateCommand(criteria);
        }
    }
}