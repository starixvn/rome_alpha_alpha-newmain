namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Filters;
    using Sunplus.Agent.Model.Entities;

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
        {
            ViewData["DarkModeName"] = userSession.DarkModeName;
            return View();
        }

        public ActionResult Blocked()
        {
            ViewData["DarkModeName"] = userSession.DarkModeName;
            return View();
        }
    }
}