namespace Alpha.Main.DataAccesses
{
    using Alpha.Main.Models.SecurityCode;
    using Criteria.SecurityCode;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface SecurityCodeDataAccess
    {
        string GetMainAccountSecurityCode(GetMainAccountSecurityCodeCriteria criteria);

        string GetSubAccountSecurityCode(GetSubAccountSecurityCodeCriteria criteria);

        Result UpdateMainAccountSecurityCodeCounter(UpdateMainAccountSecurityCodeCounterCriteria criteria);

        Result UpdateSubAccountSecurityCodeCounter(UpdateSubAccountSecurityCodeCounterCriteria criteria);

        Result UpdateMainAccountSecurityCode(UpdateMainAccountSecurityCodeCriteria criteria);

        Result UpdateSubAccountSecurityCode(UpdateSubAccountSecurityCodeCriteria criteria);

        Result ResetSubAccountSecurityCode(ResetSubAccountSecurityCodeCriteria criteria);
    }
}