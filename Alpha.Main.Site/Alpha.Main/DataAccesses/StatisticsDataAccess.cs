namespace Alpha.Main.DataAccesses
{
    using Alpha.Web.Models;
    using Fanex.Data.Repository;
    using Models.Statistics;
    using System.Collections.Generic;

    public interface StatisticsDataAccess
    {
        Statistics GetStatistics(ICriteria criteria, UserLevel userLevel);

        List<TopWinningMember> GetTopWinningMembers(ICriteria criteria);

        List<TopTurnoverMember> GetTopTurnoverMembers(ICriteria criteria);
    }
}