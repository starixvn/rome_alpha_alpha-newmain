namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

    public class DisableSecurityCodeTest : PassSecurityCodeTest
    {
        public DisableSecurityCodeTest()
        {
            SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSession.IsRequireSecurityCode.Returns(false);
        }
    }
}
