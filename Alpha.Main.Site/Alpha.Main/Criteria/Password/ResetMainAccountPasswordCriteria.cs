namespace Alpha.Main.Criteria.Password
{
    using Fanex.Data.Repository;

    public class ResetMainAccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string UserPwd { get; set; }

        public int UserId { get; set; }

        public bool DisCasino { get; set; }

        public string Language { get; set; }

        public int SubAccId { get; set; }

        public string LoginIP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "ResetMainAccountPassword";
        }

        public override bool IsValid() => CustId > 0;
    }
}