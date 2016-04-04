namespace Alpha.Main.Site.Services
{
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;

    public interface AnnouncementService
    {
        HotAnnouncement GetHotAnnouncement(HotAnnouncementQuery message);
    }
}