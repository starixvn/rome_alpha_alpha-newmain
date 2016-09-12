namespace Alpha.Main.Services
{
    using Models.SecurityCode;
    using Params.SecurityCode;
    using Sessions;
    using System.Diagnostics.CodeAnalysis;

    public interface SecurityCodeService
    {
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Reviewed!")]
        bool IsIgnoreSecurityCode(string url);

        bool IsForceUseSecurityCode(string site, SecurityCodeStatus securityCodeResult, ISecurityCodeSession securityCodeSession);

        bool ValidateSecurityCode(string securityCode);

        string MD5Salt(int custId);

        Result CompareSecurityCodeBetweenWebAndDB(CheckSecurityCodeParams inputParams);

        Result ForceChangeSecurityCodeByFirstLogin(SecurityCodeParams inputParams);

        Result ChangeSecurityCode(SecurityCodeParams inputParams);

        Result UpdateMainAccountSecurityCode(SecurityCodeParams inputParams);

        Result ResetSubAccountSecurityCodeByUplineOrMainAccount(SecurityCodeParams inputParams);

    }
}
