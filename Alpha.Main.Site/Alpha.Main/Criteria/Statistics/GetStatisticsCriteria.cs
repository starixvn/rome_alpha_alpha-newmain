namespace Alpha.Main.Criteria.Statistics
{
    using System.Collections.Generic;
    using Alpha.Web.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetStatisticsCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelStatisticsInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelStatisticsInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelStatisticsInfo_SAg"
        };

        public GetStatisticsCriteria(UserLevel userLevel, int custId)
        {
            UserLevel = userLevel;
            CustId = custId;
        }

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; }

        public int CustId { get; }

        public override string GetSettingKey() => SettingKeys[UserLevel];

        public override bool IsValid() => CustId > 0;
    }
}