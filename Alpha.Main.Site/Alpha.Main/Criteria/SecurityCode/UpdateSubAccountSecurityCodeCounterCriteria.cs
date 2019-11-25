namespace Alpha.Main.Criteria.SecurityCode
{
    public class UpdateSubAccountSecurityCodeCounterCriteria : UpdateSecurityCodeCriteria
    {
        public UpdateSubAccountSecurityCodeCounterCriteria()
        {
            SettingKey = "Age_UpdateSubAccountSecurityCodeCounter";
        }

        public int SubaccId { get; set; }

        public bool CheckedSecurityCode { get; set; }
    }
}
