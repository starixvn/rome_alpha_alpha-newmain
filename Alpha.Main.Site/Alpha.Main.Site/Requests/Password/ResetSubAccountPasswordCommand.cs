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

        public ResetSubAccountPasswordCommand(IUserSession userSession)
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
                return this.userSession.CustId;
            }
        }

        public string ActorName
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public bool IsCasinoDisabled
        {
            get
            {
                return this.userSession.IsCasinoDisabled;
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

        public string Ip
        {
            get
            {
                return this.userSession.UserLoginIp;
            }
        }
    }
}