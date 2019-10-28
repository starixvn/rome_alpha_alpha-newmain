namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public string SettingKey { get; set; }

        public override string GetSettingKey() => SettingKey;

        public override bool IsValid() => CustId > 0;
    }
}
