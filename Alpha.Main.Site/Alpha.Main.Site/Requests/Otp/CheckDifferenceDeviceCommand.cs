namespace Alpha.Main.Site.Requests.Otp
{
    using AlphaPlus.User.Models;
    using MediatR;

    public class CheckDifferenceDeviceCommand : IAsyncRequest<bool>
    {
        private readonly IUserSession userSession;

        public CheckDifferenceDeviceCommand() : this(null)
        {
        }

        public CheckDifferenceDeviceCommand(IUserSession userSession)
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
    }
}