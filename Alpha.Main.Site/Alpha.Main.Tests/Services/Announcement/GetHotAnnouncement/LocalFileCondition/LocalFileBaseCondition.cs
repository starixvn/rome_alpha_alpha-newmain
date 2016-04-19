namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LocalFileCondition
{
    using Alpha.Main.Params.Announcements;
    using Constants;
    using Helps;

    public class LocalFileBaseCondition
    {
        public const string EmptyAnnouncementPath = @"\TestData\Announcements\HotAnnouncement\EmptyAnnouncement.xml";

        public const string AvailableAnnoucementPath = @"\TestData\Announcements\HotAnnouncement\Available\Announcement_InStartEndDateRange.xml";

        public string CommonAnnouncementPath { get; set; }

        public string PrivateMessagePath { get; set; }

        public virtual void SetUpCondition(GetHotAnnouncementsParams query)
        {
            query.CommonAnnouncementPath = FileHelper.GetFilePath(CommonAnnouncementPath);
            query.PrivateMessagePath = FileHelper.GetFilePath(PrivateMessagePath);
            query.Language = Language.Default;
        }
    }
}
