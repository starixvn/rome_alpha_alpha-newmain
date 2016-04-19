namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

    public class NotRequireSecurityCodeCondition : SecurityCodeCondition
    {
        public NotRequireSecurityCodeCondition()
        {
            IsSecurityCodeActive = true;

            SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}
