#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Main.Models.Enumerations;
    using MediatR;
    using Requests.Dashboard.MemberRequests;
    using Sunplus.Agent.Model.Entities;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MemberRequestsController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public MemberRequestsController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
            => PartialView(mediator.Send(new MemberRequestsQuery(userSession)));

        [HttpGet]
        public ActionResult Guidelines()
        {
            var custLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);

            return View($"~/Views/MemberRequests/Guidelines/{userSession.Language.ToLowerInvariant()}/{custLevel.DisplayName}/Index.cshtml");
        }
    }
}