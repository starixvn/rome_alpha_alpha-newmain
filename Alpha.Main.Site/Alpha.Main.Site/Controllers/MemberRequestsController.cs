#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MemberRequestsController : BaseController
    {
        public MemberRequestsController()
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}