namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data.Repository;

    public class UpdateSecurityCodeStatusOfSubAccountCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public bool CheckedSecurityCode { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerUpdSecurityCodeStatus_Sub";
        }
    }
}