namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using NSubstitute;
    using Sessions;

    public class NotRequireSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public NotRequireSecurityCodeCondition()
        {
            IsSecurityCodeActive = true;

            SecurityCodeSession = Substitute.For<SecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}