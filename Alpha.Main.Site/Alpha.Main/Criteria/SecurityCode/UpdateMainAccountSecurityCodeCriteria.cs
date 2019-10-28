namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data;

    public class UpdateMainAccountSecurityCodeCriteria : UpdateSecurityCodeCriteria
    {
        public UpdateMainAccountSecurityCodeCriteria()
        {
            SettingKey = "Age_UpdateMainAccountSecurityCode";
        }

        public int SubaccId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }
    }
}
