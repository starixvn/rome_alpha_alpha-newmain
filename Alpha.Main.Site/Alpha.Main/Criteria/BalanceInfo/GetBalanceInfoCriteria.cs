namespace Alpha.Main.Criteria.BalanceInfo
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models.Enumerations;

    public class GetBalanceInfoCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelBalanceInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelBalanceInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelBalanceInfo_SAg"
        };

        private string settingKey;

        public GetBalanceInfoCriteria(UserLevel userLevel, int custId, DateTime moveBetsDay)
        {
            settingKey = SettingKeys[userLevel];
            CustId = custId;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; private set; }

        public DateTime MoveBetsDay { get; private set; }

        public override string GetSettingKey() => settingKey;        

        public override bool IsValid() => CustId > 0;               
    }
}