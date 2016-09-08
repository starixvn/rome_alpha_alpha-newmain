namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Alpha.SecurityCode.Sessions;
    using Main.Models.SecurityCode;
    using MediatR;
    using System.Configuration;
    using ViewModels.Security;
    public class VerifySecurityCodeCommand : IRequest<Result>
    {
        public VerifySecurityCodeCommand(string securityCode)
        {
            this.SecurityCode = securityCode;
        }

        public int CustId { get; set; }

        public string SecurityCode { get; set; }

        public string SecurityCodeToken { get; set; }

        public string Language { get; set; }

        private string IP { get; set; }

        public int SubAccountId { get; set; }
    }
}