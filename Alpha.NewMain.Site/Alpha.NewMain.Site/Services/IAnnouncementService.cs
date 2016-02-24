namespace Alpha.NewMain.Site.Services
{
    using Alpha.NewMain.Site.Models.Requests.Report;
    using Models.Views.Report;

    public interface IAnnouncementService
    {
        AnnouncementViewModel GetAnnouncements(string lastPostTimeRequest);
    }
}