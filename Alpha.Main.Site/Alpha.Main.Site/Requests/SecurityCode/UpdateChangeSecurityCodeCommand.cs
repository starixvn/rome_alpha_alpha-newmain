namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Alpha.SecurityCode.Sessions;
    using Main.Models.SecurityCode;
    using MediatR;
    using System.Configuration;
    using ViewModels.Security;
    public class UpdateChangeSecurityCodeCommand : IRequest<Result>
    {
        public UpdateChangeSecurityCodeCommand(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            this.OldSecurityCode = oldSecurityCode;
            this.SecurityCode = securityCode;
            this.ConfirmSecurityCode = confirmSecurityCode;
        }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string ConfirmSecurityCode { get; set; }

        public string SecurityCodeToken { get; set; }

        public string Language { get; set; }

        private string IP { get; set; }
    }
}