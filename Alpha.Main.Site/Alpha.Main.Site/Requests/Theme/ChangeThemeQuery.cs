namespace Alpha.Main.Site.Requests.Theme
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ChangeThemeQuery : IRequest<bool>
    {
        private readonly IUserSession userSession;

        public ChangeThemeQuery() : this(null)
        {
        }

        internal ChangeThemeQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public bool IsDarkMode { get; set; }

        public string IP => userSession.UserLoginIp;

        public string Location { get; } = "HeaderMenu";

        public bool IsInternal => userSession.IsInternal;
    }
}