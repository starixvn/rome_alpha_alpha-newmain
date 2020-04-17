namespace Alpha.Main.Site.Requests.Password
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class SkipForceChangePasswordCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public SkipForceChangePasswordCommand() : this(null, null)
        {
        }

        internal SkipForceChangePasswordCommand(IUserSession userSession, bool? isCasinoDisabled)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public string Language => userSession.Language;

        public string IP => userSession.UserLoginIp;

        public bool IsSkipForceChangePassword => userSession.IsSkipForceChangePassword;
    }
}