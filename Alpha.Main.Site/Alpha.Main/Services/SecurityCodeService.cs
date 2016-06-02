namespace Alpha.Main.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;

    public interface SecurityCodeService
    {
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Reviewed!")]
        bool IsIgnoreSecurityCode(string url);

        bool IsForceUseSecurityCode(string site, SecurityCodeStatus securityCodeResult, ISecurityCodeSession securityCodeSession);
    }
}
