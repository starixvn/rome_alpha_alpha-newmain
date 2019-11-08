namespace Alpha.Main.Site.Requests.Otp
{
    using Alpha.Main.Site.ViewModels.Security;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class VerifyOtpQuery : IRequest<VerifyOTPViewModel>
    {
        private readonly IUserSession userSession;

        public VerifyOtpQuery() : this(null)
        {
        }

        internal VerifyOtpQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }

        public string UserName
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public string SubUserName
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }
    }
}