namespace Alpha.Main.Criteria.Statistics
{
    using Fanex.Data.Repository;

    public class GetNewMembersCriteria : CriteriaBase
    {
        public GetNewMembersCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; private set; }

        public override string GetSettingKey() => "Age_CustomerSelNewMembers";

        public override bool IsValid() => CustId > 0;
    }
}