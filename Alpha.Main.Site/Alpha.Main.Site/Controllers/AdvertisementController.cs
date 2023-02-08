namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Main.Site.ViewModels.Advertisement;
    using Alpha.Web;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AdvertisementController : BaseController
    {
        private readonly IUserSession userSession;

        public AdvertisementController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult MyWatchlist()
        {
            if (userSession.SubPageAccess.ViewMemberInfo)
            {
                return PartialView(new MyWatchlistViewModel(userSession.Language, userSession.DarkModeName, userSession.RoleId));
            }

            return new EmptyResult();
        }
    }
}