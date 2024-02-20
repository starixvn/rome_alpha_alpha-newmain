namespace Alpha.Main.Criteria.SecurityCode
{
    public class UpdateSecurityCodeFunctionCriteria : UpdateSecurityCodeCriteria
    {
        public UpdateSecurityCodeFunctionCriteria(
            bool isOn,
            string function,
            int custId,
            int subAccId,
            string langKey,
            string ip)
        {
            IsOn = isOn;
            Function = function;
            CustId = custId;
            SubAccId = subAccId;
            Language = langKey;
            IP = ip;
        }

        public bool IsOn { get; }

        public string Function { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_CustomerUpdSecurityFunctionInfo";

        public override bool IsValid()
            => base.IsValid() && SubAccId >= 0;
    }
}
