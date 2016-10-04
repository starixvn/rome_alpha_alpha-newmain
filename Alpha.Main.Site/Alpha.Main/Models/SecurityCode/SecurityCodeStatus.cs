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
        None = 0,

        /// <summary>
        /// Require input SC 
        /// </summary>
        ProvideSecurityCode,

        /// <summary>
        /// Input SC successfully
        /// </summary>
        ProvideSecurityCodeSuccess,

        /// <summary>
        /// Change SC the First login
        /// </summary>
        ChangeSecurityCodeFirstLogin,

        /// <summary>
        /// Change SC by forcing
        /// </summary>
        ChangeSecurityCodeByForcing
    }
}
