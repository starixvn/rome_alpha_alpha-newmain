namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    public class LocalFileConditionFactory
    {
        public static LocalFileCondition Create(bool hasAnnouncement)
        {
            if (hasAnnouncement)
            {
                return new HasAnnouncementCondition();
            }

            return new EmptyAnnouncementCondition();
        }
    }
}
