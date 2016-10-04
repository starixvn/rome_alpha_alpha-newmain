namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data.Repository;

    public class ResetSubAccountHashSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_ResetSubAccountSecurityCode";
        }
    }
}