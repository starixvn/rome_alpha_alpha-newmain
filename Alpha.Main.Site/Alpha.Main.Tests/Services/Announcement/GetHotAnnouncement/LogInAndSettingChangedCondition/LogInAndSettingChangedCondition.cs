namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Alpha.Main.Params.Announcements;

    public class LogInAndSettingChangedCondition
    {
        public byte RoleId { get; set; }

        public int AdminId { get; set; }

        public virtual void SetUpCondition(GetHotAnnouncementsParams query)
        {
            query.RoleId = RoleId;
            query.AdminId = AdminId;
        }
    }
}
