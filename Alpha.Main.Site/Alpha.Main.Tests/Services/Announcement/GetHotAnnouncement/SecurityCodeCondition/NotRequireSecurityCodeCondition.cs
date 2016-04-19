namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

    public class NotRequireSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public NotRequireSecurityCodeCondition()
        {
            IsSecurityCodeActive = true;

            SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}
