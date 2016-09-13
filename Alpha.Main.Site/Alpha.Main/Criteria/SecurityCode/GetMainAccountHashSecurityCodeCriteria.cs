namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data.Repository;

    public class GetMainAccountHashSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_GetMainAccountSecurityCode";
        }
    }
}