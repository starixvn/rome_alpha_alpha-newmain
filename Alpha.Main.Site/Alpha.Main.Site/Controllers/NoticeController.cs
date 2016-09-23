namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;

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