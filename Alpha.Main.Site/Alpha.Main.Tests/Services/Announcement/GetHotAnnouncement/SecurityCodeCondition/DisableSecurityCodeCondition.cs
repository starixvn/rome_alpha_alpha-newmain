namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

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
