namespace Alpha.Main.Site.Requests.Otp
{
    using Alpha.Main.Site.ViewModels.Security;
    using Main.Models.Otp;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class SetUpOtpQuery : IRequest<OtpViewModel>
    {
        private readonly IUserSession userSession;

        public SetUpOtpQuery() : this(null)
        {
        }

        internal SetUpOtpQuery(IUserSession userSession)
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

        public string Username
        {
            get; set;
        }
    }
}