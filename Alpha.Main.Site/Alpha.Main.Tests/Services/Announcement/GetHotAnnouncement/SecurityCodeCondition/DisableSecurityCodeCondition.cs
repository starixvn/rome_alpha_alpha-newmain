namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using NSubstitute;
    using Sessions;

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