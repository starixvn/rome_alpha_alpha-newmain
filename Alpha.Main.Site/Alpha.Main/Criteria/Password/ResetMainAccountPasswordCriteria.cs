namespace Alpha.Main.Criteria.Password
{
    using System;
    using Fanex.Data.Repository;

    public class ResetMainAccountPasswordCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public string UserPwd { get; set; }

        public int UserID { get; set; }

        public bool DisCasino { get; set; }

        public string Language { get; set; }

        public int SubAccID { get; set; }

        public string LoginIP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerUpdPwd";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}