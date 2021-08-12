namespace Alpha.Main.Site.Requests.ProtectionCode
{
    using Alpha.Main.Site.ViewModels.ProtectionCode;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ProtectionCodeQuery : IRequest<ProtectionCodeViewModel>
    {
        private readonly IUserSession userSession;

        public ProtectionCodeQuery() : this(null)
        {
        }

        public ProtectionCodeQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public byte RoleId => userSession.RoleId;

        public string Language => userSession.Language;
    }
}