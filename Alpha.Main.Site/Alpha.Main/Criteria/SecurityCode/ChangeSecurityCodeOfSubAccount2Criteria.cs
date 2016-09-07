namespace Alpha.Main.Criteria.SecurityCode
{
    using Fanex.Data.Repository;

    public class ChangeSecurityCodeOfSubAccount2Criteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public int UserId { get; set; }

        public string SecurityCode { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerUpdSecurityCode_Sub2";
        }
    }
}