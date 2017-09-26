namespace Alpha.Main.Site.Requests.Otp
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class CheckDifferenceDeviceCommand : IRequest<bool>
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