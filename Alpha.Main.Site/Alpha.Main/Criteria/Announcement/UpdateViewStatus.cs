namespace Alpha.Main.Criteria.Announcement
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateViewStatus : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsViewed { get; set; }

        public override string GetSettingKey()
        {
            return "Age_MessageNoticeUpdStatus";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}