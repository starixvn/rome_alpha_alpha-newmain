namespace Alpha.Main.Site.Requests.Password
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ResetSubAccountPasswordCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ResetSubAccountPasswordCommand() : this(null)
        {
        }

        internal ResetSubAccountPasswordCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int SubAccountId { get; set; }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public int ActorId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public string ActorName
        {
            get
            {
                return userSession.IsSubAcc ? 
                    userSession.SubUserName : userSession.UserName;
            }
        }

        public string NewPassword { get; set; }

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

        public byte CustRoleId => userSession.RoleId;

        public int ServerId => userSession.ServerId;

        public int IndirectCustId => userSession.IndirectCustId;

        public bool IsIndirectLoggedIn => userSession.IsIndirectLoggedIn;

        public int IndirectSubAccId => userSession.IndirectSubAccId;

        public string IndirectUsername => userSession.IndirectUsername;
    }
}