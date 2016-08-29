namespace Alpha.Main.DataAccesses
{
    using Alpha.Main.Models.SecurityCode;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface SecurityCodeDataAccess
    {
        string GetSecurityCodeOfMainAccount(int custId);

        string GetSecurityCodeOfSubAccount(int custId, int subAccountId);

        Result UpdateSecurityCodeStatusOfMainAccount(
            int custId,
            bool validSecurityCode,
            string language,
            string clientIP);

        Result UpdateSecurityCodeStatusOfSubAccount(
            int custId,
            int subAccountId,
            bool validSecurityCode,
            string language,
            string clientIP);

        Result ChangeSecurityCodeOfMainAccount(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);

        Result ChangeSecurityCodeOfSubAccount(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);

        Result ChangeSecurityCodeOfSubAccount2(
            int custId,
            int userId,
            int subAccountId,
            string securityCode,
            string language,
            string clientIP);
    }
}
