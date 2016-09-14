namespace Alpha.Main.Models.SecurityCode
{
    /// <summary>
    /// Security code status.
    /// </summary>
    public enum SecurityCodeStatus
    {
        /// <summary>
        /// The NONE value.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The PROVIDE_ SC.
        /// </summary>
        PROVIDED_SECURITYCODE,

        /// <summary>
        /// The PROVIDED_SC_SUCCESS.
        /// </summary>
        PROVIDED_SECURITYCODE_SUCCESS,

        /// <summary>
        /// The FORCED_CHANGE_SC_WITHOUT_VALIDATION.
        /// </summary>
        CHANGE_SECURITYCODE_FIRSTLOGIN,

        /// <summary>
        /// The FORCED_CHANGE_SC.
        /// </summary>
        CHANGE_SECURITYCODE_BY_FORCING
    }
}
