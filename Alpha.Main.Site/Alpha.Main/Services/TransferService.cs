namespace Alpha.Main.Services
{
    public interface TransferService
    {
        bool IsAllDayBalanceTransfer(int custId);
        bool IsEnableAutoTransfer(string currency, int custId, string offerCurrencies);
    }
}
