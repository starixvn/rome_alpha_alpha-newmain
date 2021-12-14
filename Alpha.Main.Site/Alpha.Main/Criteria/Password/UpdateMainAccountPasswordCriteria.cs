namespace Alpha.Main.Criteria.Password
{
    using Fanex.Data.Repository;

    public class UpdateMainAccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string UserPwd { get; set; }

        public string OldPwd { get; set; }

        public string LoginIP { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "UpdateMainAccountPassword";
        }

        public override bool IsValid() => CustId > 0;
    }
}