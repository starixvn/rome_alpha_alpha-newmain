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
            bool isSuperOrMaster = userLevel.IsMasterLevel() || userLevel.IsSuperLevel();

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
                       outStanding: !Convert.IsDBNull(reader["outStanding"]) ? Convert.ToDecimal(reader["outStanding"]) : 0,
                       totalOutStanding: !Convert.IsDBNull(reader["total_outstanding"]) ? Convert.ToDecimal(reader["total_outstanding"]) : 0,
                       totalMemberActive: !Convert.IsDBNull(reader["total_memberActive"]) ? Convert.ToInt32(reader["total_memberActive"]) : 0,
                       totalMemberClosed: !Convert.IsDBNull(reader["total_memberClosed"]) ? Convert.ToInt32(reader["total_memberClosed"]) : 0,
                       totalMemberSuspended: !Convert.IsDBNull(reader["total_memberSuspended"]) ? Convert.ToInt32(reader["total_memberSuspended"]) : 0,
                       totalAgentActive: isSuperOrMaster && !Convert.IsDBNull(reader["total_agentActive"]) ? Convert.ToInt32(reader["total_agentActive"]) : 0,
                       totalAgentClosed: isSuperOrMaster && !Convert.IsDBNull(reader["total_agentClosed"]) ? Convert.ToInt32(reader["total_agentClosed"]) : 0,
                       totalAgentSuspended: isSuperOrMaster && !Convert.IsDBNull(reader["total_agentSuspended"]) ? Convert.ToInt32(reader["total_agentSuspended"]) : 0,
                       totalMasterActive: userLevel.IsSuperLevel() && !Convert.IsDBNull(reader["total_masterActive"]) ? Convert.ToInt32(reader["total_masterActive"]) : 0,
                       totalMasterClosed: userLevel.IsSuperLevel() && !Convert.IsDBNull(reader["total_masterClosed"]) ? Convert.ToInt32(reader["total_masterClosed"]) : 0,
                       totalMasterSuspended: userLevel.IsSuperLevel() && !Convert.IsDBNull(reader["total_masterSuspended"]) ? Convert.ToInt32(reader["total_masterSuspended"]) : 0,
                       numberNewMembers: !Convert.IsDBNull(reader["number_newMembers"]) ? Convert.ToInt32(reader["number_newMembers"]) : 0);
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
                        custId: !Convert.IsDBNull(reader["custid"]) ? Convert.ToInt32(reader["custid"]) : 0,
                        userName: !Convert.IsDBNull(reader["username"]) ? Convert.ToString(reader["username"]) : string.Empty,
                        winLoss: !Convert.IsDBNull(reader["winlost"]) ? Convert.ToDecimal(reader["winlost"]) : 0);

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
                        custId: !Convert.IsDBNull(reader["custid"]) ? Convert.ToInt32(reader["custid"]) : 0,
                        userName: !Convert.IsDBNull(reader["username"]) ? Convert.ToString(reader["username"]) : string.Empty,
                        turnOver: !Convert.IsDBNull(reader["turnover"]) ? Convert.ToDecimal(reader["turnover"]) : 0);

                    topTurnoverMembers.Add(turnoverMember);
                }
            }

            return topTurnoverMembers;
        }
    }
}