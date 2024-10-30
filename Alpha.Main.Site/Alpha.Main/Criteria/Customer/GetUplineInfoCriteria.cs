using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.Customer
{
    public class GetUplineInfoCriteria : CriteriaBase
    {
        public GetUplineInfoCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_CustomerSelUplineInfo";

        public override bool IsValid() => CustId > 0;
    }
}