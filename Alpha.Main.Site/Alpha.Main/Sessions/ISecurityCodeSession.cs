namespace Alpha.Main.Sessions
{
    public interface ISecurityCodeSession
    {
        string Language { get; }

        int SubAccId { get; }

        int CustId { get; }

        int UserId { get; }

        byte RoleId { get; }

        bool IsInternal { get; }

        bool IsForcedSecurityCode { get; }

        string SecurityCode { get; }

        string SecurityCodeToken { get; set; }

        int SecurityCodeResult { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is enable security code.
        /// Check PROVIDED_SC_SUCCESS status also.
        /// </summary>
        /// <value>
        /// <c>true</c> If this instance is enable security code; otherwise, <c>false</c>.
        /// </value>
        bool IsEnableSecurityCode { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is require security code.
        /// Do not check PROVIDED_SC_SUCCESS status.
        /// </summary>
        /// <value>
        /// <c>true</c> If this instance is require security code; otherwise, <c>false</c>.
        /// </value>
        bool IsRequireSecurityCode { get; }
    }
}
