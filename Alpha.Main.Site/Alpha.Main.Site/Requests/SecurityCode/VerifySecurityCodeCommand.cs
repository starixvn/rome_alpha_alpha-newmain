namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    using Sessions;

    public class VerifySecurityCodeCommand : IRequest<Result>
    {
        private ISecurityCodeSession securityCodeSession;

        public VerifySecurityCodeCommand() : this(null)
        {
        }

        public VerifySecurityCodeCommand(ISecurityCodeSession securityCodeSession)
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
                return this.securityCodeSession.SubAccId.Equals(0);
            }
        }
    }
}