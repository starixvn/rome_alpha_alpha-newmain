namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data;

    public class ResetSubAccountSecurityCodeCriteria : UpdateSecurityCodeCriteria
    {
        public ResetSubAccountSecurityCodeCriteria()
        {
            SettingKey = "Age_ResetSubAccountSecurityCode";
        }

        public int SubaccId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }

        public int UserId { get; set; }
    }
}
