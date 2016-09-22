namespace Alpha.Main.DataAccesses
{
    using Fanex.Data.Repository;
    using Models;

    public interface PasswordDataAccess
    {
        Result UpdatePassword(ICriteria criteria);
    }
}