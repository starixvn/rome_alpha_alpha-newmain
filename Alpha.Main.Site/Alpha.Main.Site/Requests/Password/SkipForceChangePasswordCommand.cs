namespace Alpha.Main.Site.Requests.Password
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class SkipForceChangePasswordCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;
        private readonly bool isCasinoDisabled;

        public SkipForceChangePasswordCommand() : this(null, null)
        {
        }

        internal SkipForceChangePasswordCommand(IUserSession userSession, bool? isCasinoDisabled)
        {
            this.userSession = userSession ?? new UserSessionExt();

            // Line below follow old business, please do not modify.
            this.isCasinoDisabled = isCasinoDisabled.HasValue ? isCasinoDisabled.Value : Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino;
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
                return this.userSession.UserId;
            }
        }

        public bool IsCasinoDisabled
        {
            get
            {
                return isCasinoDisabled;
            }
        }

        public string ConfirmMessage { get; set; }

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

        public bool IsSkipForceChangePassword
        {
            get
            {
                return this.userSession.IsSkipForceChangePassword;
            }
        }
    }
}