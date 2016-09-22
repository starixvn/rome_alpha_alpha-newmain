
namespace Alpha.Main.Criteria.HomeInfo
{
    using System;
    using Fanex.Data.Repository;

    public class GetBalanceUpDayInfoCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_SelBalanceUpDay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
