namespace Alpha.Main.Criteria.Statistics
{
    using System;
    using System.Collections.Generic;
    using Alpha.Web.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetTopWinningMembersCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelTopWinningMembers_Ag",
            [UserLevel.Master] = "Age_CustomerSelTopWinningMembers_MAg",
            [UserLevel.Super] = "Age_CustomerSelTopWinningMembers_SAg"
        };

        public GetTopWinningMembersCriteria(
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