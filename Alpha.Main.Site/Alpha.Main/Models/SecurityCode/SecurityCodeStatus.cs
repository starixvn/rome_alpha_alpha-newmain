namespace Alpha.Main.Models.SecurityCode
{
    /// <summary>
    /// Security code status.
    /// </summary>
    public enum SecurityCodeStatus
    {
        /// <summary>
        /// Not offer SC
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Require input SC 
        /// </summary>
        PROVIDED_SECURITYCODE,

        /// <summary>
        /// Input SC successfully
        /// </summary>
        PROVIDED_SECURITYCODE_SUCCESS,

        /// <summary>
        /// Change SC the First login
        /// </summary>
        CHANGE_SECURITYCODE_FIRSTLOGIN,

        /// <summary>
        /// Change SC by forcing
        /// </summary>
        CHANGE_SECURITYCODE_BY_FORCING
    }
}
