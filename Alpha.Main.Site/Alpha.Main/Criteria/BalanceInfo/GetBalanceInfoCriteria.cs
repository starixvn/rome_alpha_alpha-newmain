namespace Alpha.Main.Criteria.BalanceInfo
{
    using Alpha.Web.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;

    public class GetBalanceInfoCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelBalanceInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelBalanceInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelBalanceInfo_SAg"
        };

        private readonly string settingKey;

        public GetBalanceInfoCriteria(UserLevel userLevel, int custId, DateTime moveBetsDay)
        {
            settingKey = SettingKeys[userLevel];
            CustId = custId;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => settingKey;

        public override bool IsValid() => CustId > 0;
    }
}