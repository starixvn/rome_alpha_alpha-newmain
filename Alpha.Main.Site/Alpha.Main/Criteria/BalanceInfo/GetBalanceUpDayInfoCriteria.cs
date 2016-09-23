namespace Alpha.Main.Criteria.BalanceInfo
{
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