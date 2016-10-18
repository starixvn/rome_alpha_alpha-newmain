namespace Alpha.Main.Criteria.Password
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateSubAccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string UserPwd { get; set; }

        public string OldPwd { get; set; }

        public string LoginIP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "UpdateSubAccountPassword";
        }
    }
}