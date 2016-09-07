namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data.Repository;

    public class GetSecurityCodeOfMainAccountCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelSecurityCode";
        }
    }
}