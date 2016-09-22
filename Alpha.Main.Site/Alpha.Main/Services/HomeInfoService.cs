namespace Alpha.Main.Services
{
    using Models.Home;
    using Sunplus.Agent.Model.Entities;
    using System;

    public interface HomeInfoService
    {
        BalanceInfo GetBalanceInfo(IUserSession currentUser);
    }
}
