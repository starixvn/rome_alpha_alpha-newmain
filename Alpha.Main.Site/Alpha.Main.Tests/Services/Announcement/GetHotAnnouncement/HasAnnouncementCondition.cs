namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    public class HasAnnouncementCondition : LocalFileCondition
    {
        public HasAnnouncementCondition()
        {
            CommonAnnouncementPath = AvailableAnnoucementPath;
            PrivateMessagePath = EmptyAnnouncementPath;
        }
    }
}
