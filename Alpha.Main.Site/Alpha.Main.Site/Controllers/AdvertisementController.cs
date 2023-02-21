namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Main.Site.ViewModels.Advertisement;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AdvertisementController : BaseController
    {
        private readonly IUserSession userSession;

        public AdvertisementController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        [AllowProtectedMode]
        public ActionResult MyWatchlist()
        {
            if (userSession.SubPageAccess.ViewMemberInfo && !userSession.IsProtectedMode)
            {
                return PartialView(new MyWatchlistViewModel(userSession.Language, userSession.DarkModeName, userSession.RoleId));
            }

            return new EmptyResult();
        }
    }
}