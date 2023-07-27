namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Main.Site.Helpers;
    using Alpha.Web.Filters;
    using AlphaPlus.User.Models;

    [AllowAnonymous]
    [AllowProtectedMode]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ErrorController : Controller
    {
        private readonly IUserSession userSession;

        public ErrorController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult Index()
            => View((object)DarkModeHelper.GetDarkModeName(userSession));

        public ActionResult Blocked()
            => View((object)DarkModeHelper.GetDarkModeName(userSession));
    }
}