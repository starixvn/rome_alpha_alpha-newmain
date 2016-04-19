namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    public class NotAgentDirectLoginCondition : LogInAndSettingChangedCondition
    {
        private const int ValidAdminId = 1;

        public NotAgentDirectLoginCondition(byte roleId = Alpha.Main.Constants.RoleId.ValidRoleId, bool isAdminLogIn = true)
        {
            RoleId = roleId;
            AdminId = isAdminLogIn ? ValidAdminId : 0;
        }
    }
}
