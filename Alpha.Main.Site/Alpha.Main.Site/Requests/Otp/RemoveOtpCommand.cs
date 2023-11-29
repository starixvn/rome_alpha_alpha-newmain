namespace Alpha.Main.Site.Requests.Otp
{
    using AlphaPlus.User.Models;
    using Main.Models;
    using MediatR;

    public class RemoveOtpCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public RemoveOtpCommand(IUserSession userSession)
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

        public string Language
        {
            get
            {
                return this.userSession.Language;
            }
        }

        public string IP
        {
            get
            {
                return this.userSession.UserLoginIp;
            }
        }
    }
}