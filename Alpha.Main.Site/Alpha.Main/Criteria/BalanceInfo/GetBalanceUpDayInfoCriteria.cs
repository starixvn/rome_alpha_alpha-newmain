namespace Alpha.Main.Criteria.BalanceInfo
{
    using Fanex.Data.Repository;

    public class GetBalanceUpDayInfoCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_SelBalanceUpDay";

        public override bool IsValid() => true;
    }
}