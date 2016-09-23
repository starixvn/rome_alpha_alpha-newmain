namespace Alpha.Main.Criteria.Statistics
{
    using System;
    using Fanex.Data.Repository;
    using Models.Enumerations;

    public class GetTopWinningMembers : CriteriaBase
    {
        private const string SettingKeyAgent = "Age_CustomerSelTopWinningMembers_Ag";
        private const string SettingKeyMaster = "Age_CustomerSelTopWinningMembers_MAg";
        private const string SettingKeySuper = "Age_CustomerSelTopWinningMembers_SAg";

        private string settingKey;

        public GetTopWinningMembers(UserLevel userLevel)
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