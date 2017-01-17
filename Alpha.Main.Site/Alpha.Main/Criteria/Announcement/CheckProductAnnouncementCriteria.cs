namespace Alpha.Main.Criteria.Announcement
{
    using Fanex.Data.Repository;

    public class CheckProductAnnouncementCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string OfferedSite { get; set; }

        public bool IsLastBatch { get; set; }

        public bool IsShowAnnouncement { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerShowAnnouncement";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
