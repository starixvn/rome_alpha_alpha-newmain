namespace Alpha.Main.Criteria.Announcement
{
    using System;
    using Fanex.Data.Repository;

    public class GetMessageTypeListCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_MessageNoticeSelByCustID";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}