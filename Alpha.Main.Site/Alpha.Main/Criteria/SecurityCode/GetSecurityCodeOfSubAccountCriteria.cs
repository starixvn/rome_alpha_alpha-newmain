namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data.Repository;

    public class GetSecurityCodeOfSubAccountCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerUpdSecurityCodeStatus";
        }
    }
}