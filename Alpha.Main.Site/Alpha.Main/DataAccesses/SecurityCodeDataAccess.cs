namespace Alpha.Main.DataAccesses
{
    using Fanex.Data.Repository;
    using Models;

    public interface SecurityCodeDataAccess
    {
        string GetHashSecurityCode(ICriteria criteria);

        Result UpdateSecurityCode(ICriteria criteria);
    }
}