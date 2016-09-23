namespace Alpha.Main.Services
{
    using Models.Enumerations;
    using Models.Statistics;
    using System.Collections.Generic;

    public interface StatisticsService
    {
        Statistics GetStatistics(int custId, UserLevel userLevel);

        IEnumerable<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel);

        IEnumerable<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel);

        IEnumerable<NewMember> GetNewMembers(int custId);
    }
}
