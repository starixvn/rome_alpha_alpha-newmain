namespace Alpha.Main.Site.Requests.Otp
{
    using Alpha.Main.Models;
    using AlphaPlus.User.Models;
    using MediatR;

    public class VerifyOtpCommand : IAsyncRequest<Result>
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

        public string Language
        {
            get
            {
                return this.userSession.Language;
            }
        }
    }
}