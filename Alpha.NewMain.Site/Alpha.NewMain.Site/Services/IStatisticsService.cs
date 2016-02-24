namespace Alpha.NewMain.Site.Services
{
    using Alpha.NewMain.Site.Models.Views.Report.Statistics;

    public interface IStatisticsService
    {
        StatisticsViewModel GetStatistics();

        Top10WinningOfMonthViewModel Top10WinningOfMonth();

        HighestTurnoverOfMonthViewModel HighestTurnoverOfMonth();

        NewMembersOfMonthViewModel NewMembersOfMonth();
    }
}