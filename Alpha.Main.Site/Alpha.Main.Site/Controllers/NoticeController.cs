namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;

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