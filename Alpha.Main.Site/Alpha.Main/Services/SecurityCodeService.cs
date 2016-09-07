namespace Alpha.Main.Services
{
    using Models.SecurityCode;
    using Params.Announcements;
    using SecurityCode.Entities;
    using SecurityCode.Sessions;
    using System.Diagnostics.CodeAnalysis;

    public interface SecurityCodeService
    {
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Reviewed!")]
        bool IsIgnoreSecurityCode(string url);

        bool IsForceUseSecurityCode(string site, SecurityCodeStatus securityCodeResult, ISecurityCodeSession securityCodeSession);

        bool ValidateSecurityCode(string securityCode);

        string GenerateSecurityToken(int size = 6);

        string MD5Salt(int custId);

        Result CheckSecurityCode(SecurityCodeParams inputParams);

        Result ChangeSecurityCodeWithoutValidation(SecurityCodeParams inputParams);

        Result ChangeSecurityCode(SecurityCodeParams inputParams);

        Result ResetSecurityCode(SecurityCodeParams inputParams);

        Result ResetSecurityCodeOfSubAccount(SecurityCodeParams inputParams);

    }
}
