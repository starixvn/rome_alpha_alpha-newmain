namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using NSubstitute;

    public class DisableSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public DisableSecurityCodeCondition()
        {
            IsSecurityCodeActive = false;
        }
    }
}