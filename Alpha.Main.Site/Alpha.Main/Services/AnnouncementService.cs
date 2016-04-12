namespace Alpha.Main.Services
{
    using Models.Announcements;
    using Params.Announcements;

    public interface AnnouncementService
    {
        HotAnnouncement GetHotAnnouncement(GetHotAnnouncementsParams getParams);
    }
}