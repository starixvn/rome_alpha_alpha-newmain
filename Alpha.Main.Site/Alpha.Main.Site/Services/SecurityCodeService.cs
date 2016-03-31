namespace Alpha.Main.Site.Services
{
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;

    public interface SecurityCodeService
    {
        bool IsIgnoreSecurityCode(string url);

        bool IsForceUseSecurityCode(string site, SecurityCodeStatus securityCodeResult, ISecurityCodeSession securityCodeSession);
    }
}
