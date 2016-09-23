namespace Alpha.Main.Criteria.Statistics
{
    using Fanex.Data.Repository;

    public class GetNewMembersCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelNewMembers";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}