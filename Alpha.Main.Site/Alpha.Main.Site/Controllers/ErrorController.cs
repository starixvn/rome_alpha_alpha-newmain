namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Filters;

    [AllowAnonymous]
    [AllowProtectedMode]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Blocked()
        {
            return View();
        }
    }
}