namespace Alpha.Main.Site.Services
{
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;

    public interface IAnnouncementService
    {
        HotAnnouncementModel GetHotAnnouncement(HotAnnouncementQuery message);
    }
}