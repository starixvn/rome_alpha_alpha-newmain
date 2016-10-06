namespace Alpha.Main.Criteria.Statistics
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models.Enumerations;

    public class GetTopTurnoverMembersCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelTopTurnoverMembers_Ag",
            [UserLevel.Master] = "Age_CustomerSelTopTurnoverMembers_MAg",
            [UserLevel.Super] = "Age_CustomerSelTopTurnoverMembers_SAg"
        };

        private readonly string settingKey;

        public GetTopTurnoverMembersCriteria(
            UserLevel userLevel,
            int custId,
            DateTime balanceUpDay,
            DateTime moveBetsDay)
        {
            settingKey = SettingKeys[userLevel];
            CustId = custId;
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => settingKey;

        public override bool IsValid() => CustId > 0;
    }
}