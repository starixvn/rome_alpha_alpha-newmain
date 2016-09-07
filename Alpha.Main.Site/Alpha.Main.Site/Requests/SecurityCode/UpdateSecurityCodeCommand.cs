namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Alpha.SecurityCode.Sessions;
    using Main.Models.SecurityCode;
    using MediatR;
    using System.Configuration;
    using ViewModels.Security;
    public class UpdateSecurityCodeCommand : IRequest<Result>
    {
        public int CustId { get; set; }

        public int UserId { get; set; }

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string RandomKey { get; set; }

        public string Language { get; set; }

        private string IP
        {
            get
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                    ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string[] ips = string.IsNullOrEmpty(ip) ? new string[] { string.Empty } : ip.Split(',');

                return ips[0];
            }
        }

        public int SubAccountId { get; set; }

        public bool IsResetMember { get; set; }
    }
}