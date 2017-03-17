#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.Dashboard.MemberRequests;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MemberRequestsController : BaseController
    {
        private readonly IMediator mediator;

        public MemberRequestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = mediator.Send(new MemberRequestsQuery());
            return PartialView(viewModel);
        }
    }
}