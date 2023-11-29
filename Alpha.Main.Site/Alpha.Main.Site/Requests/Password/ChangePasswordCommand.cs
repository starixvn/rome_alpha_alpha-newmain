namespace Alpha.Main.Site.Requests.Password
{
    using AlphaPlus.User.Models;
    using Main.Models;
    using MediatR;

    public class ChangePasswordCommand : IRequest<Result>
    {
        protected readonly IUserSession userSession;

        public ChangePasswordCommand() : this(null)
        {
        }

        internal ChangePasswordCommand(IUserSession userSession)
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

        public byte RoleId
        {
            get
            {
                return userSession.RoleId;
            }
        }

        public int ActorId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public string NewPassword { get; set; }

        public string CurrentPassword { get; set; }

        public string LanguageKey
        {
            get
            {
                return this.userSession.LangKey;
            }
        }

        public string IP
        {
            get
            {
                return this.userSession.UserLoginIp;
            }
        }

        public bool IsSubAccount
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }

        public string Username
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public string SubAccountUsername
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public bool IsIndirectLoggedIn => userSession.IsIndirectLoggedIn;

        public int IndirectCustId => userSession.IndirectCustId;

        public int IndirectSubAccId => userSession.IndirectSubAccId;

        public string IndirectUsername => userSession.IndirectUsername;
    }
}