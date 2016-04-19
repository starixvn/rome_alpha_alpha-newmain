namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement.SecurityCodeCondition
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

    public class NotPassSecurityCodeCondition : SecurityCodeBaseCondition
    {
        public NotPassSecurityCodeCondition()
        {
            IsSecurityCodeActive = true;

            SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(true);
        }
    }
}
