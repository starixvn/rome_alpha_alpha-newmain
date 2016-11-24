namespace Alpha.Main.Criteria.Announcement
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateViewStatus : CriteriaBase
    {
        public UpdateViewStatus(int custId, bool isViewed)
        {
            CustId = custId;
            IsViewed = isViewed;
        }

        public int CustId { get; }

        public bool IsViewed { get; }

        public override string GetSettingKey()
        {
            return "Age_MessageNoticeUpdStatus";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}