namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NoticeController : BaseController
    {
        public NoticeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}