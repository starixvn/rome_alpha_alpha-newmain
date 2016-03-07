namespace Alpha.Main.Site.Criteria.Announcement
{
    using Fanex.Data.Repository;

    public class MessageTypeListCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_MessageNoticeSelByCustID";
        }
    }
}