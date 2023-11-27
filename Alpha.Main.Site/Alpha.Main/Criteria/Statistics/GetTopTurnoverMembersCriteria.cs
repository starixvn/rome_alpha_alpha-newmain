namespace Alpha.Main.Criteria.Statistics
{
    using System;
    using System.Collections.Generic;
    using Alpha.Web.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetTopTurnoverMembersCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelTopTurnoverMembers_Ag",
            [UserLevel.Master] = "Age_CustomerSelTopTurnoverMembers_MAg",
            [UserLevel.Super] = "Age_CustomerSelTopTurnoverMembers_SAg"
        };

        public GetTopTurnoverMembersCriteria(
            UserLevel userLevel,
            int custId,
            DateTime balanceUpDay,
            DateTime moveBetsDay)
        {
            UserLevel = userLevel;
            CustId = custId;
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; }

        public int CustId { get; }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => SettingKeys[UserLevel];

        public override bool IsValid() => CustId > 0;
    }
}