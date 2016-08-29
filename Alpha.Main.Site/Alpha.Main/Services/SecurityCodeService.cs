namespace Alpha.Main.Services
{
    using Models.SecurityCode;
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

        Result CheckSecurityCode(
            int custId,
            string securityCode,
            string randomKey,
            string language,
            string clientIP,
            int subAccountId = 0);

        Result ChangeSecurityCodeWithoutValidation(
            int custId,
            int userId,
            string securityCode,
            string language,
            string clientIP,
            int subAccountId = 0);

        Result ChangeSecurityCode(
            int custId,
            int userId,
            string oldSecurityCode,
            string securityCode,
            string randomKey,
            string language,
            string clientIP,
            int subAccountId = 0);

        Result ResetSecurityCode(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP,
            bool isResetMember = false);

        Result ResetSecurityCodeOfSubAccount(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);

    }
}
