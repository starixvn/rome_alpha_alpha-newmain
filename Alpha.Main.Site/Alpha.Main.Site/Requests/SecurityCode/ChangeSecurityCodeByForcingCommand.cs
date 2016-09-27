namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    using Sessions;

    public class ChangeSecurityCodeByForcingCommand : IRequest<Result>
    {
        private ISecurityCodeSession securityCodeSession;

        public ChangeSecurityCodeByForcingCommand() : this(null)
        {
        }

        public ChangeSecurityCodeByForcingCommand(ISecurityCodeSession securityCodeSession)
        {
            this.securityCodeSession = securityCodeSession ?? new SecurityCodeSession();
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

        public string OldSecurityCode { get; set; }

        public string ConfirmSecurityCode { get; set; }

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