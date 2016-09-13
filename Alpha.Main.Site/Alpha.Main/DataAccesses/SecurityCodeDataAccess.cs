namespace Alpha.Main.DataAccesses
{
    using Alpha.Main.Models.SecurityCode;
    using Criteria.SecurityCode;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface SecurityCodeDataAccess
    {
        string GetMainAccountHashSecurityCode(GetMainAccountHashSecurityCodeCriteria criteria);

        string GetSubAccountHashSecurityCode(GetSubAccountHashSecurityCodeCriteria criteria);

        Result UpdateMainAccountSecurityCodeCounter(UpdateMainAccountSecurityCodeCounterCriteria criteria);

        Result UpdateSubAccountSecurityCodeCounter(UpdateSubAccountSecurityCodeCounterCriteria criteria);

        Result UpdateMainAccountHashSecurityCode(UpdateMainAccountHashSecurityCodeCriteria criteria);

        Result UpdateSubAccountHashSecurityCode(UpdateSubAccountHashSecurityCodeCriteria criteria);

        Result ResetSubAccountHashSecurityCode(ResetSubAccountHashSecurityCodeCriteria criteria);
    }
}