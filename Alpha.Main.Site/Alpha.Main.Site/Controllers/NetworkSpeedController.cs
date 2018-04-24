namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.NetworkSpeed;
    using Sunplus.Agent.Model.Entities;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NetworkSpeedController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public NetworkSpeedController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult Index()
        {
            return View(mediator.Send(new NetworkSpeedQuery(userSession.CustId)));
        }

        [HttpGet]
        public JsonResult SaveResult(SaveNetworkSpeedCommand command)
        {
            mediator.Send(command);

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}