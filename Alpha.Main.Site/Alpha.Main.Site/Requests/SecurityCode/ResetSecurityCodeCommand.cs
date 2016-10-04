namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models;
    using MediatR;
    using Sessions;

    public class ResetSecurityCodeCommand : IRequest<Result>
    {
        private SecurityCodeSession securityCodeSession;

        public ResetSecurityCodeCommand() : this(null)
        {
        }

        public ResetSecurityCodeCommand(SecurityCodeSession securityCodeSession)
        {
            this.securityCodeSession = securityCodeSession ?? new SecurityCodeSessionImpl();
        }

        public int? CustId { get; set; }

        public int UserId
        {
            get
            {
                return this.securityCodeSession.UserId;
            }
        }

        public int? SubAccId { get; set; }

        public bool IsSubAcc
        {
            get
            {
                return !this.securityCodeSession.SubAccId.Equals(0);
            }
        }

        public string SecurityCode { get; set; }

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