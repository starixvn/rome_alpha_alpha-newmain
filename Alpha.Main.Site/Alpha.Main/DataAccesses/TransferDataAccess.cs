namespace Alpha.Main.DataAccesses
{
    using Fanex.Data.Repository;

    public interface TransferDataAccess
    {
        bool IsAllDayBalanceTransfer(ICriteria criteria);
    }
}
