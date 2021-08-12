namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.ProtectionCode;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ProtectionCodeController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public ProtectionCodeController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult Index(ProtectionCodeQuery query)
            => View(mediator.Send(query));

        [HttpPost]
        [AntiXsrfFilter]
        public async Task<ActionResult> Update(UpdateProtectionCodeCommand query)
            => Json(await mediator.SendAsync(query));
    }
}