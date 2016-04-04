namespace Alpha.Main.Site.Services
{
    using Models.Announcements;
    using Models.Requests.Announcement;

    public interface AnnouncementService
    {
        HotAnnouncement GetHotAnnouncement(HotAnnouncementQuery message);
    }
}