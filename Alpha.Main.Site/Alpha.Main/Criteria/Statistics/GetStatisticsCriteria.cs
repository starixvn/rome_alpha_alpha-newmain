namespace Alpha.Main.Criteria.Statistics
{
    using Alpha.Web.Models;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class GetStatisticsCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelStatisticsInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelStatisticsInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelStatisticsInfo_SAg"
        };

        private readonly string settingKey;

        public GetStatisticsCriteria(UserLevel userLevel, int custId)
        {
            settingKey = SettingKeys[userLevel];
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => settingKey;

        public override bool IsValid() => CustId > 0;
    }
}