namespace Alpha.Main.Site.Requests.ProtectionCode
{
    using Alpha.Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class UpdateProtectionCodeCommand : IAsyncRequest<Result>
    {
        private readonly IUserSession userSession;

        public UpdateProtectionCodeCommand() : this(null)
        {
        }

        public UpdateProtectionCodeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public string ProtectionCode { get; set; }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public string LangKey => userSession.LangKey;

        public string IP => userSession.UserLoginIp;
    }
}