namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using NSubstitute;
    using SecurityCode.Sessions;
    public class DisableSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public DisableSecurityCodeCondition()
        {
            IsSecurityCodeActive = false;

            SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}
