namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    using Sessions;
    public class UpdateChangeSecurityCodeCommand : IRequest<Result>
    {
        private ISecurityCodeSession securityCodeSession;

        public UpdateChangeSecurityCodeCommand(string oldSecurityCode, string securityCode, string confirmSecurityCode, ISecurityCodeSession securityCodeSession)
        {
            this.securityCodeSession = securityCodeSession;
            this.OldSecurityCode = oldSecurityCode;
            this.SecurityCode = securityCode;
            this.ConfirmSecurityCode = confirmSecurityCode;
        }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string ConfirmSecurityCode { get; set; }

        public string SecurityCodeToken
        {    get
            {
                return this.securityCodeSession.SecurityCodeToken;
            }
        }

        public string Language { get; set; }

        private string IP { get; set; }
    }
}