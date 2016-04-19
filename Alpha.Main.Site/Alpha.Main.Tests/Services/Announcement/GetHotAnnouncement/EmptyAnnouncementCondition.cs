namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    public class EmptyAnnouncementCondition : LocalFileCondition
    {
        public EmptyAnnouncementCondition()
        {
            CommonAnnouncementPath = EmptyAnnouncementPath;
            PrivateMessagePath = EmptyAnnouncementPath;
        }
    }
}
