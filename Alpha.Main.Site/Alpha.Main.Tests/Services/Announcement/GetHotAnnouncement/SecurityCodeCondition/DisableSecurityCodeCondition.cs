namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using NSubstitute;
    using Sessions;

    public class DisableSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public DisableSecurityCodeCondition()
        {
            IsSecurityCodeActive = false;

            SecurityCodeSession = Substitute.For<SecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}