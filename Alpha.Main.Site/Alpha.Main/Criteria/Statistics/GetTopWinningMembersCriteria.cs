namespace Alpha.Main.Criteria.Statistics
{
    using Alpha.Web.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;

    public class GetTopWinningMembersCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelTopWinningMembers_Ag",
            [UserLevel.Master] = "Age_CustomerSelTopWinningMembers_MAg",
            [UserLevel.Super] = "Age_CustomerSelTopWinningMembers_SAg"
        };

        private readonly string settingKey;

        public GetTopWinningMembersCriteria(
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