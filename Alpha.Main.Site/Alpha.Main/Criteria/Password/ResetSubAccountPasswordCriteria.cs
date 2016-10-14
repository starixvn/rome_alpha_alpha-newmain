namespace Alpha.Main.Criteria.Password
{
    using System;
    using Fanex.Data.Repository;

    public class ResetSubAccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string UserPwd { get; set; }

        public string Language { get; set; }

        public int LoginSubAccId
        {
            get
            {
                //// Hard code because store procedured do nothing with this param
                return 0;
            }
        }

        public string LoginIP { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "ResetSubAccountPassword";
        }
    }
}