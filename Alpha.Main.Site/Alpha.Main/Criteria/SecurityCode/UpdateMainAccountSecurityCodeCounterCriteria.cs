namespace Alpha.Main.Criteria.SecurityCode
{
    public class UpdateMainAccountSecurityCodeCounterCriteria : UpdateSecurityCodeCriteria
    {
        public UpdateMainAccountSecurityCodeCounterCriteria()
        {
            SettingKey = "Age_UpdateMainAccountSecurityCodeCounter";
        }

        public bool CheckedSecurityCode { get; set; }
    }
}
