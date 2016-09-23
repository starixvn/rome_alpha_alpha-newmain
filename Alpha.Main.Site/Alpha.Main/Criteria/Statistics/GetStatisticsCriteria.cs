namespace Alpha.Main.Criteria.Statistics
{
    using Fanex.Data.Repository;
    using Models.Enumerations;

    public class GetStatisticsCriteria : CriteriaBase
    {
        private const string SettingKeyAgent = "Age_CustomerSelStatisticsInfo_Ag";
        private const string SettingKeyMaster = "Age_CustomerSelStatisticsInfo_MAg";
        private const string SettingKeySuper = "Age_CustomerSelStatisticsInfo_SAg";

        private string settingKey;

        public GetStatisticsCriteria(UserLevel userLevel)
        {
            this.settingKey = InitSettingKey(userLevel);
        }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return this.settingKey;
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        private string InitSettingKey(UserLevel userLevel)
        {
            if (userLevel.IsAgentLevel())
            {
                return SettingKeyAgent;
            }
            else if (userLevel.IsMasterLevel())
            {
                return SettingKeyMaster;
            }
            else if (userLevel.IsSuperLevel())
            {
                return SettingKeySuper;
            }

            return string.Empty;
        }
    }
}
