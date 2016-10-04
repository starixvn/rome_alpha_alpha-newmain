namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models;
    using MediatR;
    using Sessions;

    public class ChangeUserOwnSecurityCodeCommand : IRequest<Result>
    {
        private SecurityCodeSession securityCodeSession;

        public ChangeUserOwnSecurityCodeCommand() : this(null)
        {
        }

        public ChangeUserOwnSecurityCodeCommand(SecurityCodeSession securityCodeSession)
        {
            this.securityCodeSession = securityCodeSession ?? new SecurityCodeSessionImpl();
        }

        public int CustId
        {
            get
            {
                return this.securityCodeSession.CustId;
            }
        }

        public int UserId
        {
            get
            {
                return this.securityCodeSession.UserId;
            }
        }

        public int SubAccId
        {
            get
            {
                return this.securityCodeSession.SubAccId;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return !this.securityCodeSession.SubAccId.Equals(0);
            }
        }

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string SecurityCodeToken
        {
            get
            {
                return this.securityCodeSession.SecurityCodeToken;
            }
        }

        public string Language
        {
            get
            {
                return this.securityCodeSession.Language;
            }
        }

        private string IP { get; set; }
    }
}