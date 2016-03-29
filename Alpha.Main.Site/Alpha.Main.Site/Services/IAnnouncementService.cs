namespace Alpha.Main.Site.Services
{
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;

    public interface IAnnouncementService
    {
        HotAnnouncement GetHotAnnouncement(HotAnnouncementQuery message);
    }
}