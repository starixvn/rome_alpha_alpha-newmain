namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web;
    using Alpha.Web.Filters;

    public class PartialUMController : BaseController
    {
        [AllowProtectedMode]
        public ActionResult Index() => View();
    }
}