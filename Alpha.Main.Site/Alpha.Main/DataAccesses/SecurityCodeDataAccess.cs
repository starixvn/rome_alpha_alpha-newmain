namespace Alpha.Main.DataAccesses
{
    using Fanex.Data.Repository;
    using Models;

    public interface SecurityCodeDataAccess
    {
        string GetSecurityCode(ICriteria criteria);

        Result UpdateSecurityCode(ICriteria criteria);
    }
}