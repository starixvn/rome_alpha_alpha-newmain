namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data;

    public class UpdateSubAccountSecurityCodeCriteria : UpdateSecurityCodeCriteria
    {
        public UpdateSubAccountSecurityCodeCriteria()
        {
            SettingKey = "Age_UpdateSubAccountSecurityCode";
        }

        public int SubaccId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }
    }
}
