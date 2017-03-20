namespace Alpha.Main.Criteria.MemberRequests
{
    using Fanex.Data.Repository;

    public class ProductRequestCriteria : CriteriaBase
    {
        public ProductRequestCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_CustomerSelMemberRequest";

        public override bool IsValid() => CustId > 0;
    }
}