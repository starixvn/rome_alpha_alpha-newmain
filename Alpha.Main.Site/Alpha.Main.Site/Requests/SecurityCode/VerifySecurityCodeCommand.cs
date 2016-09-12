namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    using Sessions;
    public class VerifySecurityCodeCommand : IRequest<Result>
    {
        private ISecurityCodeSession securityCodeSession;
        public VerifySecurityCodeCommand(string securityCode, ISecurityCodeSession securityCodeSession)
        {
            this.securityCodeSession = securityCodeSession;
            this.SecurityCode = securityCode;
        }

        public int CustId { get; set; }

        public string SecurityCode { get; set; }

        public string SecurityCodeToken
        {
            get
            {
                return this.securityCodeSession.SecurityCodeToken;
            }
        }

        public string Language { get; set; }

        private string IP { get; set; }

        public int SubAccountId { get; set; }
    }
}