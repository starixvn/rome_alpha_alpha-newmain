namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.LocalFileCondition
{
    public class LocalFileConditionFactory
    {
        public static LocalFileBaseCondition Create(bool hasAnnouncement)
        {
            if (hasAnnouncement)
            {
                return new HasAnnouncementCondition();
            }

            return new EmptyAnnouncementCondition();
        }
    }
}
