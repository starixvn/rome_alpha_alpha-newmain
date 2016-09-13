namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    using Sessions;
    public class ResetSecurityCodeCommand : IRequest<Result>
    {
        private ISecurityCodeSession securityCodeSession;
        public ResetSecurityCodeCommand(int? custId, int? SubAccId, string securityCode, string confirmSecurityCode, ISecurityCodeSession securityCodeSession)
        {
            this.CustId = custId;
            this.SubAccId = SubAccId;
            this.securityCodeSession = securityCodeSession;
            this.SecurityCode = securityCode;
            this.ConfirmSecurityCode = confirmSecurityCode;
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

        public string ConfirmSecurityCode { get; set; }

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