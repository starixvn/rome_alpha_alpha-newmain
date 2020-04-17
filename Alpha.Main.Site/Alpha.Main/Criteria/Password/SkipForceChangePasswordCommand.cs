namespace Alpha.Main.Criteria.Password
{
    using Fanex.Data.Repository;

    public class SkipForceChangePasswordCommand : NonQueryCommand
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string LoginIP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerSkipForceUpdPwd";

        public override bool IsValid() => CustId > 0;
    }
}