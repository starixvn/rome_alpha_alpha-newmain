namespace Alpha.NewMain.Site.Services
{
    using System.Collections.Generic;
    using Models.Dtos;

    public interface IReportService
    {
        BalanceUpDayModel GetBalanceUpDay();

        BalanceInfoModel GetBalanceInfo();
    }
}
