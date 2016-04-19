namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LocalFileCondition
{
    public class EmptyAnnouncementCondition : LocalFileBaseCondition
    {
        public EmptyAnnouncementCondition()
        {
            CommonAnnouncementPath = EmptyAnnouncementPath;
            PrivateMessagePath = EmptyAnnouncementPath;
        }
    }
}
