namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web;

    public class PartialUMController : BaseController
    {
        public ActionResult Index() => View();
    }
}