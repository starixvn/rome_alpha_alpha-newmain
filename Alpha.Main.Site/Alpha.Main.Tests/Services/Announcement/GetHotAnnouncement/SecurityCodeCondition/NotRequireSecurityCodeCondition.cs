namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using NSubstitute;

    public class NotRequireSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public NotRequireSecurityCodeCondition()
        {
            IsSecurityCodeActive = true;
        }
    }
}