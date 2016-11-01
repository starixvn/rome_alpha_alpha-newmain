namespace Alpha.Main.Criteria.SecurityCode
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateSubAccountSecurityCodeCounterCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public bool CheckedSecurityCode { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_UpdateSubAccountSecurityCodeCounter";
        }

   

        public override bool IsValid() => CustId > 0;
    }
}