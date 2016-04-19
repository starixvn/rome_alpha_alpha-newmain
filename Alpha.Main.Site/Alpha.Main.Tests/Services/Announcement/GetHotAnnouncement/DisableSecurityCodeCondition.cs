namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

    public class DisableSecurityCodeCondition : SecurityCodeCondition
    {
        public DisableSecurityCodeCondition()
        {
            IsSecurityCodeActive = false;

            SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}
