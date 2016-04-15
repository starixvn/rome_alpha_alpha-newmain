namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;

    public class PassSecurityCodeTest
    {
        public ISecurityCodeSession SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
    }
}
