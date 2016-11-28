namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models.Enumerations;
    using Models.Statistics;

    public class StatisticsDataAccessImpl : StatisticsDataAccess
    {
        public Statistics GetStatistics(ICriteria criteria, UserLevel userLevel)
        {
            Statistics statistics = new Statistics();

            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());

                if (reader.Read())
                {
                    statistics = new Statistics(
                       outStanding: Convert.ToDecimal(reader["outStanding"] ?? 0),
                       totalOutStanding: Convert.ToDecimal(reader["total_outstanding"] ?? 0),
                       totalMemberActive: Convert.ToInt32(reader["total_memberActive"] ?? 0),
                       totalMemberClosed: Convert.ToInt32(reader["total_memberClosed"] ?? 0),
                       totalMemberSuspended: Convert.ToInt32(reader["total_memberSuspended"] ?? 0),
                       totalAgentActive: userLevel.IsMasterLevel() || userLevel.IsSuperLevel() ? Convert.ToInt32(reader["total_agentActive"] ?? 0) : 0,
                       totalAgentClosed: userLevel.IsMasterLevel() || userLevel.IsSuperLevel() ? Convert.ToInt32(reader["total_agentClosed"] ?? 0) : 0,
                       totalAgentSuspended: userLevel.IsMasterLevel() || userLevel.IsSuperLevel() ? Convert.ToInt32(reader["total_agentSuspended"] ?? 0) : 0,
                       totalMasterActive: userLevel.IsSuperLevel() ? Convert.ToInt32(reader["total_masterActive"] ?? 0) : 0,
                       totalMasterClosed: userLevel.IsSuperLevel() ? Convert.ToInt32(reader["total_masterClosed"] ?? 0) : 0,
                       totalMasterSuspended: userLevel.IsSuperLevel() ? Convert.ToInt32(reader["total_masterSuspended"] ?? 0) : 0,
                       numberNewMembers: Convert.ToInt32(reader["number_newMembers"] ?? 0));
                }
            }

            return statistics;
        }

        public List<TopWinningMember> GetTopWinningMembers(ICriteria criteria)
        {
            var topWinningMembers = new List<TopWinningMember>();

            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());

                while (reader.Read())
                {
                    var winningMember = new TopWinningMember(
                        custId: Convert.ToInt32(reader["custid"] ?? 0),
                        userName: Convert.ToString(reader["username"] ?? string.Empty),
                        winLoss: Convert.ToDecimal(reader["winlost"] ?? 0));

                    topWinningMembers.Add(winningMember);
                }
            }

            return topWinningMembers;
        }

        public List<TopTurnoverMember> GetTopTurnoverMembers(ICriteria criteria)
        {
            var topTurnoverMembers = new List<TopTurnoverMember>();

            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());

                while (reader.Read())
                {
                    var turnoverMember = new TopTurnoverMember(
                        custId: Convert.ToInt32(reader["custid"] ?? 0),
                        userName: Convert.ToString(reader["username"] ?? string.Empty),
                        turnOver: Convert.ToDecimal(reader["turnover"] ?? 0));

                    topTurnoverMembers.Add(turnoverMember);
                }
            }

            return topTurnoverMembers;
        }
    }
}