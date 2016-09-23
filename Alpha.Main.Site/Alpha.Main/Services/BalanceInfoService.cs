namespace Alpha.Main.Services
{
    using Models.BalanceInfo;
    using Models.Enumerations;

    public interface BalanceInfoService
    {
        BalanceInfo GetBalanceInfo(int custId, UserLevel userLevel);
    }
}