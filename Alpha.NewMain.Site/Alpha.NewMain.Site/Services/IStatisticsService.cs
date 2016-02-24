namespace Alpha.NewMain.Site.Services
{
    using System.Collections.Generic;
    using Models.Dtos;
    using Models.Dtos.Statistics;

    public interface IStatisticsService
    {
        StatisticsModel GetStatistics();

        IEnumerable<CustomerInfoModel> Top10WinningOfMonth();

        IEnumerable<CustomerInfoModel> HighestTurnoverOfMonth();

        IEnumerable<CustomerInfoModel> NewMembersOfMonth();
    }
}