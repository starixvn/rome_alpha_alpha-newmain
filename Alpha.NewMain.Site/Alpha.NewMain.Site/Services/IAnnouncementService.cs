namespace Alpha.NewMain.Site.Services
{
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;

    public interface IAnnouncementService
    {
        AnnouncementReportModel GetAnnouncements(string lastPostTimeRequest, int totalAnnouncement);

        HotAnnouncementModel GetHotAnnouncement(HotAnnouncementQuery message);
    }
}