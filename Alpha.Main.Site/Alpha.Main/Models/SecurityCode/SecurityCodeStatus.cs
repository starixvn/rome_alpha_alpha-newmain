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
        /// Change SC the First login
        /// </summary>
        ChangeSecurityCodeFirstLogin,

        /// <summary>
        /// Change SC by forcing
        /// </summary>
        ChangeSecurityCodeByForcing
    }
}
