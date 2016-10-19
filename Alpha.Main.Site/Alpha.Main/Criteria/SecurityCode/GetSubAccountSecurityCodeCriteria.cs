namespace Alpha.Main.Criteria.SecurityCode
{
    using System;
    using Fanex.Data.Repository;

    public class GetSubAccountSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_GetSubAccountSecurityCode";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}