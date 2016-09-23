namespace Alpha.Main.Criteria.BalanceInfo
{
    using System;
    using Fanex.Data.Repository;
    using Models.Enumerations;

    public class GetBalanceInfoCriteria : CriteriaBase
    {
        private const string SettingKeyAgent = "Age_CustomerSelBalanceInfo_Ag";
        private const string SettingKeyMaster = "Age_CustomerSelBalanceInfo_MAg";
        private const string SettingKeySuper = "Age_CustomerSelBalanceInfo_SAg";

        private string settingKey;

        public GetBalanceInfoCriteria(UserLevel userLevel)
        {
            this.settingKey = InitSettingKey(userLevel);
        }

        public int CustId { get; set; }

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