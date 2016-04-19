namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LocalFileCondition
{
    public class HasAnnouncementCondition : LocalFileBaseCondition
    {
        public HasAnnouncementCondition()
        {
            CommonAnnouncementPath = AvailableAnnoucementPath;
            PrivateMessagePath = EmptyAnnouncementPath;
        }
    }
}
