namespace Alpha.NewMain.Site.Services
{
    using Models.Dtos;
    using Models.Views.Report;
    public interface IReportService
    {
        BalanceUpDayModel GetBalanceUpDay();

        BalanceInfoViewModel GetBalanceInfo();
    }
}
