namespace Alpha.Main.Site.Requests.Password
{
    using Alpha.Web.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using CustomResult = Alpha.Main.Models.Result;

    public class ResetMainAccountPasswordCommand : IRequest<CustomResult>
    {
        private readonly IUserSession userSession;
        private readonly bool isCasinoDisabled;

        public ResetMainAccountPasswordCommand() : this(null, null)
        {
        }

        internal ResetMainAccountPasswordCommand(IUserSession userSession, bool? isCasinoDisabled)
        {
            this.userSession = userSession ?? new UserSessionExt();

            // Line below follow old business, please do not modify.
            this.isCasinoDisabled = isCasinoDisabled.HasValue ? isCasinoDisabled.Value : Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino;
        }

        public int CustId { get; set; }

        public int ActorId
        {
            get
            {
                return this.userSession.UserId;
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
                return isCasinoDisabled;
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

        public bool IsCasinoSync { get; set; }

        public string ActorName
        {
            get
            {
                return this.userSession.IsSubAcc ? this.userSession.SubUserName : this.userSession.UserName;
            }
        }

        public byte CustRoleId { get; set; }

        public int ServerId => userSession.ServerId;

        public int IndirectCustId => userSession.IndirectCustId;

        public bool IsIndirectLoggedIn => userSession.IsIndirectLoggedIn;

        public int IndirectSubAccId => userSession.IndirectSubAccId;

        public string IndirectUsername => userSession.IndirectUsername;

        public bool IsValid() => UserLevel.Member.Value <= CustRoleId && CustRoleId <= UserLevel.Super.Value;
    }
}