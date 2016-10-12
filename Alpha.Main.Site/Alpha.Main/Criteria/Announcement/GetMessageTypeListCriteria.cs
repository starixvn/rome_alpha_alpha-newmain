namespace Alpha.Main.Criteria.Announcement
{
    using Fanex.Data.Repository;

    public class GetMessageTypeListCriteria : CriteriaBase
    {
        public GetMessageTypeListCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; private set; }

        public override string GetSettingKey()
        {
            return "Age_MessageNoticeSelByCustID";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}