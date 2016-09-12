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
        PROVIDED_SC,

        /// <summary>
        /// The PROVIDED_SC_SUCCESS.
        /// </summary>
        PROVIDED_SC_SUCCESS,

        /// <summary>
        /// The FORCED_CHANGE_SC_WITHOUT_VALIDATION.
        /// </summary>
        FORCED_CHANGE_SC_WITHOUT_VALIDATION,

        /// <summary>
        /// The FORCED_CHANGE_SC.
        /// </summary>
        FORCED_CHANGE_SC
    }
}
