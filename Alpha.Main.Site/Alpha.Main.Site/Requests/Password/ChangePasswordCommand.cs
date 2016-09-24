namespace Alpha.Main.Site.Requests.Password
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ChangePasswordCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ChangePasswordCommand() : this(null)
        {
        }

        public ChangePasswordCommand(IUserSession userSession)
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

        public int ActorId
        {
            get
            {
                return this.userSession.CustId;
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

        public string CurrentPassword { get; set; }

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

        public bool IsCasinoSync
        {
            get
            {
                return this.userSession.IsCasinoSync;
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
    }
}