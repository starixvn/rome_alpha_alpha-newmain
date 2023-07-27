#pragma warning disable 612, 618

namespace Alpha.Main.Site.Requests
{
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using MediatR;

    public class BrowserQuery : IRequest<BrowserViewModel>
    {
        private readonly IUserSession userSession;

        public BrowserQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public byte UserLevelId => userSession.RoleId;
    }
}