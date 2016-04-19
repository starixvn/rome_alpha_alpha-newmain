namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LocalFileCondition
{
    public class HasPrivateOrCommonAnnouncementCondition : LocalFileBaseCondition
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
