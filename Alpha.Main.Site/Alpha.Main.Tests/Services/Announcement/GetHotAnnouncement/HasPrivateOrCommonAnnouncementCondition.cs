namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    public class HasPrivateOrCommonAnnouncementCondition : LocalFileCondition
    {
        public HasPrivateOrCommonAnnouncementCondition(
            string commonAnnouncementPath = EmptyAnnouncementPath,
            string privateMessagePath = EmptyAnnouncementPath)
        {
            CommonAnnouncementPath = commonAnnouncementPath ?? EmptyAnnouncementPath;
            PrivateMessagePath = privateMessagePath ?? EmptyAnnouncementPath;
        }
    }
}
