namespace Alpha.Main.Criteria.SecurityCode
{
    using System;
    using Fanex.Data.Repository;

    public class GetMainAccountSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_GetMainAccountSecurityCode";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}