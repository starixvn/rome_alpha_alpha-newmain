namespace Alpha.Main.Criteria.BalanceInfo
{
    using System;
    using System.Collections.Generic;
    using Alpha.Web.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBalanceInfoCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelBalanceInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelBalanceInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelBalanceInfo_SAg"
        };

        public GetBalanceInfoCriteria(UserLevel userLevel, int custId, DateTime moveBetsDay)
        {
            UserLevel = userLevel;
            CustId = custId;
            MoveBetsDay = moveBetsDay;
        }

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; }

        public int CustId { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => SettingKeys[UserLevel];

        public override bool IsValid() => CustId > 0;
    }
}