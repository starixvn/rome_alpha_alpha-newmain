namespace Alpha.Main.Site.Requests.Otp
{
    using Alpha.Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class VerifyOtpCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public VerifyOtpCommand() : this(null)
        {
        }

        public VerifyOtpCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public int SubAccountId
        {
            get
            {
                return userSession.SubAccId;
            }
        }

        public string Otp { get; set; }
    }
}