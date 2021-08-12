namespace Alpha.Main.Criteria.ProtectionCode
{
    using Fanex.Data.Repository;

    public class UpdateProtectionCodeCriteria : NonQueryCommand
    {
        public UpdateProtectionCodeCriteria(
            string protectionCode,
            int custId,
            int subAccId,
            string langKey,
            string ip)
        {
            ProtectionCode = protectionCode;
            CustId = custId;
            SubAccId = subAccId;
            LangKey = langKey;
            IP = ip;
        }

        public string ProtectionCode { get; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string LangKey { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0;
    }
}
