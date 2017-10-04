namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}