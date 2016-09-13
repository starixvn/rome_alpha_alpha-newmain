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

        string GetMainAccountHashSecurityCode(int custId);

        string GetSubAccountHashSecurityCode(int custId, int subAccountId);

        Result UpdateMainAccountHashSecurityCodeCounter(SecurityCodeParams securityCodeParams);

        Result UpdateSubAccountHashSecurityCodeCounter(SecurityCodeParams securityCodeParams);

        Result UpdateMainAccountHashSecurityCode(SecurityCodeParams securityCodeParams);

        Result UpdateSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams);

        Result ResetSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams);
    }
}
