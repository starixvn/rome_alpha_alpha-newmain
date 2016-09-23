namespace Alpha.Main.Services
{
    using System.Collections.Generic;
    using Models.Enumerations;
    using Models.Statistics;

    public interface StatisticsService
    {
        Statistics GetStatistics(int custId, UserLevel userLevel);

        IEnumerable<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel);

        IEnumerable<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel);

        IEnumerable<NewMember> GetNewMembers(int custId);
    }
}