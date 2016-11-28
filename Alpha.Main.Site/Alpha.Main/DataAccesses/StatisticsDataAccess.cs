namespace Alpha.Main.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models.Enumerations;
    using Models.Statistics;

    public interface StatisticsDataAccess
    {
        Statistics GetStatistics(ICriteria criteria, UserLevel userLevel);

        List<TopWinningMember> GetTopWinningMembers(ICriteria criteria);

        List<TopTurnoverMember> GetTopTurnoverMembers(ICriteria criteria);
    }
}