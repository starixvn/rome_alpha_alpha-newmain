namespace Alpha.Main.Site.Requests.Password
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ResetMainAccountPasswordCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ResetMainAccountPasswordCommand() : this(null)
        {
        }

        public ResetMainAccountPasswordCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId { get; set; }

        public int ActorId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public int SubAccountId
        {
            get
            {
                return this.userSession.SubAccId;
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

        public bool IsCasinoSync { get; set; }

        public string ActorName
        {
            get
            {
                return this.userSession.IsSubAcc ? this.userSession.SubUserName : this.userSession.UserName;
            }
        }

        public int CustRole { get; set; }
    }
}