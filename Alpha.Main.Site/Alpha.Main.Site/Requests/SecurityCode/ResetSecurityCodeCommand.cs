namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models.SecurityCode;
    using MediatR;
    public class ResetSecurityCodeCommand : IRequest<Result>
    {
        public ResetSecurityCodeCommand(int custId, int subAccId, string securityCode, string confirmSecurityCode)
        {
            this.SecurityCode = securityCode;
            this.ConfirmSecurityCode = confirmSecurityCode;
            this.CustId = custId;
            this.SubAccountId = subAccId;
        }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string SecurityCode { get; set; }

        public string ConfirmSecurityCode { get; set; }

        public string Language { get; set; }

        private string IP { get; set; }

        public int SubAccountId { get; set; }

        public bool IsResetMember { get; set; }
    }
}