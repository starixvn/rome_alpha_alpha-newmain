namespace Alpha.Main.Services
{
    using Models.SecurityCode;
    using Params.SecurityCode;
    using Sessions;
    using System.Diagnostics.CodeAnalysis;

    public interface SecurityCodeService
    {
        string GetMainAccountHashSecurityCode(int custId);

        string GetSubAccountHashSecurityCode(int custId, int subAccountId);

        Result UpdateMainAccountHashSecurityCode(SecurityCodeParams securityCodeParams);

        Result UpdateSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams);

        Result ResetSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams);

        string MD5Salt(int custId);

        bool ValidateSecurityCode(string securityCode);

        Result CompareHashSecurityCodeBetweenWebAndDB(int custId, int subAccId, bool isSubAcc, string securityCodeToken, string language, string webHashSecurityCode);
    }
}
