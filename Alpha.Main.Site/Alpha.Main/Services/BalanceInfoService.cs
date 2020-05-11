namespace Alpha.Main.Services
{
    using Alpha.Web.Models;
    using Models.BalanceInfo;

    public interface BalanceInfoService
    {
        BalanceInfo GetBalanceInfo(int custId, UserLevel userLevel);
    }
}