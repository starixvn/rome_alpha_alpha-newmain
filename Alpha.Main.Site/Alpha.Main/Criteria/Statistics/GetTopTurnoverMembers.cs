namespace Alpha.Main.Criteria.Statistics
{
    using Fanex.Data.Repository;
    using Models.Enumerations;
    using System;

    public class GetTopTurnoverMembers : CriteriaBase
    {
        private const string SettingKeyAgent = "Age_CustomerSelTopTurnoverMembers_Ag";
        private const string SettingKeyMaster = "Age_CustomerSelTopTurnoverMembers_MAg";
        private const string SettingKeySuper = "Age_CustomerSelTopTurnoverMembers_SAg";

        private string settingKey;

        public GetTopTurnoverMembers(UserLevel userLevel)
        {
            this.settingKey = InitSettingKey(userLevel);
        }

        public int CustId { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

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

            if (userLevel.IsMasterLevel())
            {
                return SettingKeyMaster;
            }

            if (userLevel.IsSuperLevel())
            {
                return SettingKeySuper;
            }

            return string.Empty;
        }
    }
}
