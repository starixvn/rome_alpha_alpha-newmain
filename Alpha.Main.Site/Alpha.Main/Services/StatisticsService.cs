namespace Alpha.Main.Services
{
    using Alpha.Web.Models;
    using Models.Statistics;
    using System.Collections.Generic;

    public interface StatisticsService
    {
        Statistics GetStatistics(int custId, UserLevel userLevel);

        List<TopWinningMember> GetTopWinningMembers(int custId, UserLevel userLevel);

        List<TopTurnoverMember> GetTopTurnoverMembers(int custId, UserLevel userLevel);

        List<NewMember> GetNewMembers(int custId);
    }
}