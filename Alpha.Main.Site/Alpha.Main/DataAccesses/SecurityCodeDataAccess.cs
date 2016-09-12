namespace Alpha.Main.DataAccesses
{
    using Alpha.Main.Models.SecurityCode;
    using Criteria.SecurityCode;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface SecurityCodeDataAccess
    {
        string GetSecurityCodeOfMainAccount(GetSecurityCodeOfMainAccountCriteria criteria);

        string GetSecurityCodeOfSubAccount(GetSecurityCodeOfSubAccountCriteria criteria);

        Result UpdateSecurityCodeStatusOfMainAccount(UpdateSecurityCodeStatusOfMainAccountCriteria criteria);

        Result UpdateSecurityCodeStatusOfSubAccount(UpdateSecurityCodeStatusOfSubAccountCriteria criteria);

        Result ChangeSecurityCodeOfMainAccount(ChangeSecurityCodeOfMainAccountCriteria criteria);

        Result ChangeSecurityCodeOfSubAccount(ChangeSecurityCodeOfSubAccountCriteria criteria);

        Result ChangeSecurityCodeOfSubAccount2(ChangeSecurityCodeOfSubAccount2Criteria criteria);
    }
}