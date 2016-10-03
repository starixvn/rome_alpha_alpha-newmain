namespace Alpha.Main.Services
{
    using System.Collections.Generic;
    using Models.Enumerations;
    using Models.Statistics;

    public interface StatisticsService
    {
        Statistics GetStatistics(int custId, UserLevel userLevel);

        List<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel);

        List<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel);

        List<NewMember> GetNewMembers(int custId);
    }
}