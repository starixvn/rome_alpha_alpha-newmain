namespace Alpha.Main.Site.Requests
{
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class BrowserQuery : IRequest<BrowserViewModel>
    {
        private readonly IUserSession userSession;

        public BrowserQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public byte UserLevelId
        {
            get
            {
                return this.userSession.RoleId;
            }
        }
    }
}