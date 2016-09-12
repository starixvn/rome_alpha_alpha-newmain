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

        string GetMainAccountSecurityCode(int custId);

        string GetSubAccountSecurityCode(int custId, int subAccountId);

        Result UpdateMainAccountSecurityCodeCounter(SecurityCodeParams securityCodeParams);

        Result UpdateSubAccountSecurityCodeCounter(SecurityCodeParams securityCodeParams);

        Result UpdateMainAccountSecurityCode(SecurityCodeParams securityCodeParams);

        Result UpdateSubAccountSecurityCode(SecurityCodeParams securityCodeParams);

        Result ResetSubAccountSecurityCode(SecurityCodeParams securityCodeParams);
    }
}
